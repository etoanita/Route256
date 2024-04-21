using Dapper;
using Ozon.Route256.Practice.OrderService.Infrastructure.Database.Repositories;
using Ozon.Route256.Practice.OrderService.Domain;
using Ozon.Route256.Practice.OrderService.Dal.Common.Shard;
using Ozon.Route256.Practice.OrderService.Dal.Models;
using OrderState = Ozon.Route256.Practice.OrderService.Dal.Models.OrderState;
using OrderType = Ozon.Route256.Practice.OrderService.Dal.Models.OrderType;
using SortOrder = Ozon.Route256.Practice.OrderService.Dal.Models.SortOrder;

namespace Ozon.Route256.Practice.OrderService.DataAccess.Postgres
{
    public class ShardOrdersDbAccess : BaseShardRepository, IOrdersRepositoryPg
    {
        private const string Fields = "id, items_count, total_price, total_weight, order_type, order_date, region_name, state, customer_id";
        private const string FieldsForInsert = "id, items_count, total_price, total_weight, order_type, order_date, region_name, state, customer_id";
        private const string Table = $"{ShardsHelper.BucketPlaceholder}.orders";

        ILogger<ShardOrdersDbAccess> _logger;

        public ShardOrdersDbAccess(IShardPostgresConnectionFactory connectionFactory,
            IShardingRule<long> shardingRule,
            IShardingRule<string> stringShardingRule,
            ILogger<ShardOrdersDbAccess> logger)
             : base(connectionFactory, shardingRule, stringShardingRule)
        {
            _logger = logger;
        }
        public async Task<OrderDal?> Find(long id, CancellationToken token = default)
        {
            const string sql = @$"
                select {Fields}
                from {Table} orders
                where id = :id;
            ";

            await using var connection = GetConnectionByShardKey(id);
            var param = new DynamicParameters();
            param.Add("id", id);
            var cmd = new CommandDefinition(sql, param, cancellationToken: token);
            return await connection.QueryFirstOrDefaultAsync<OrderDal>(cmd);
        }

        public async Task<IReadOnlyCollection<OrderDal>> FindByCustomerId(int customerId, DateTime startFrom, PaginationParameters pp, CancellationToken token = default)
        {
            const string sqlIndex = @$"
                select order_id
                from {ShardsHelper.BucketPlaceholder}.idx_orders_customer_id
                where customer_id = :customer_id and order_date > :start_from offset :skip limit :take; 
            ";

            IEnumerable<int> orders = new List<int>();
            await using (var connection = GetConnectionByShardKey(customerId)) {
                var param = new DynamicParameters();
                param.Add("customer_id", customerId);
                param.Add("start_from", startFrom);
                param.Add("skip", (pp.PageNumber - 1) * pp.PageSize);
                param.Add("take", pp.PageSize);
                var cmd = new CommandDefinition(sqlIndex, param, cancellationToken: token);
                orders = await connection.QueryAsync<int>(cmd);
            }
            List<OrderDal> result = new();
            const string sql = @$"
                select {Fields}
                from {Table} orders
                where id = :order_id;";
            for (int i = 0; i < orders.Count(); i++)
            {
                await using (var connection = GetConnectionByShardKey(i))
                {
                    var param = new DynamicParameters();
                    param.Add("order_id", orders.ElementAt(i));
                    var cmd = new CommandDefinition(sql, param, cancellationToken: token);
                    result.AddRange(await connection.QueryAsync<OrderDal>(cmd));
                } 
            }
            return result;
        }

        public async Task<IReadOnlyCollection<OrderByRegionDal>> FindByRegions(List<string> regions, DateTime startFrom, CancellationToken token = default)
        {
            const string sqlIndex = @$"
                select order_id
                from {ShardsHelper.BucketPlaceholder}.idx_orders_region_name
                where region_name = :region and order_date > :start_from; 
            ";
            List<int> ordersId = new();
            foreach (var region in regions)
            {
                await using (var connection = GetConnectionBySearchKey(region))
                {
                    var param = new DynamicParameters();
                    param.Add("region", region);
                    param.Add("start_from", startFrom);
                    var cmd = new CommandDefinition(sqlIndex, param, cancellationToken: token);
                    ordersId.AddRange(await connection.QueryAsync<int>(cmd));
                }
            }
            const string sql = @$"
                select {Fields}
                from {Table} orders
                where id = :id;
            ";
            List<OrderDal> result = new List<OrderDal>();
            foreach (var orderId in ordersId)
            {
                await using (var connection = GetConnectionByShardKey(orderId))
                {
                    var param = new DynamicParameters();
                    param.Add("id", orderId);
                    var cmd = new CommandDefinition(sql, param, cancellationToken: token);
                    result.AddRange(await connection.QueryAsync<OrderDal>(cmd));
                }
            }
            return result.AsEnumerable().GroupBy(x => x.RegionName).Select(x => new OrderByRegionDal
            (
                x.Key, x.Count(), x.Sum(y => y.TotalPrice),
                x.Sum(y => y.TotalWeight), x.Select(y => y.CustomerId).Distinct().Count())
            ).ToList();
        }

        public async Task<IReadOnlyCollection<OrderDal>> Find(List<string> regions, OrderType orderType, PaginationParameters pp, SortOrder? sortOrder, List<string> sortingFields, CancellationToken token = default)
        {
            const string sqlIndex = @$"
                select order_id
                from {ShardsHelper.BucketPlaceholder}.idx_orders_region_name
                where region_name = :region and order_type > :order_type; 
            ";
            List<int> ordersId = new();
            foreach (var region in regions)
            {
                await using (var connection = GetConnectionBySearchKey(region))
                {
                    var param = new DynamicParameters();
                    param.Add("region", region);
                    param.Add("order_type", orderType);
                    var cmd = new CommandDefinition(sqlIndex, param, cancellationToken: token);
                    ordersId.AddRange(await connection.QueryAsync<int>(cmd));
                }
            }
            sortOrder ??= SortOrder.ASC;
            sortingFields = sortingFields.Any() ? sortingFields : new List<string> { "region_name" };
            string sql = @$"
                select {Fields}
                from {Table}
                where id = :id; 
            ";

            List<OrderDal> result = new List<OrderDal>();
            foreach (var orderId in ordersId)
            {
                await using (var connection = GetConnectionByShardKey(orderId))
                {
                    var param = new DynamicParameters();
                    param.Add("id", orderId);

                    var cmd = new CommandDefinition(sql, param, cancellationToken: token);
                    result.AddRange(await connection.QueryAsync<OrderDal>(cmd));
                }
            }

            if (sortOrder != null)
            {
                result = SortByColumns(result, sortOrder.Value, sortingFields).ToList();
            }
            return result.Skip((pp.PageNumber - 1) * pp.PageSize).Take(pp.PageSize).ToList();
        }

        public async Task<OrderState> GetOrderState(long id, CancellationToken token = default)
        {
            const string sql = @$"
                select state
                from {Table} orders
                where id = :id;
            ";

            await using (var connection = GetConnectionByShardKey(id))
            {
                var param = new DynamicParameters();
                param.Add("id", id);

                var cmd = new CommandDefinition(sql, param, cancellationToken: token);
                return await connection.QueryFirstOrDefaultAsync<OrderState>(cmd);
            }
        }

        public async Task Insert(OrderDal order, CancellationToken token = default)
        {
            const string sql = @$"
            insert into {Table} ({FieldsForInsert})
            values (:id, :items_count, :total_price, :total_weight, :order_type, :order_date, :region_name, :state, :customer_id);";

            var param = new DynamicParameters();
            param.Add("id", order.Id);
            param.Add("items_count", order.ItemsCount);
            param.Add("total_price", order.TotalPrice);
            param.Add("total_weight", order.TotalWeight);
            param.Add("order_type", order.OrderType);
            param.Add("order_date", order.OrderDate);
            param.Add("region_name", order.RegionName);
            param.Add("state", order.State);
            param.Add("customer_id", order.CustomerId);

            await using (var connection = GetConnectionByShardKey(order.Id))
            {
                var cmd = new CommandDefinition(sql, param, cancellationToken: token);
                await connection.ExecuteAsync(cmd);
            }

            const string indexSql = $@"
            insert into  {ShardsHelper.BucketPlaceholder}.idx_orders_customer_id 
            (order_id, customer_id, order_date)
            VALUES (:order_id, :customer_id, :order_date)";
            param = new DynamicParameters();
            param.Add("order_id", order.Id);
            param.Add("customer_id", order.CustomerId);
            param.Add("order_date", order.OrderDate);
            await using (var connection = GetConnectionByShardKey(order.CustomerId))
            {
                await connection.ExecuteAsync(indexSql, param);
            }

            const string indexSql2 = $@"
            insert into  {ShardsHelper.BucketPlaceholder}.idx_orders_region_name 
            (order_id, region_name, order_date, order_type)
            VALUES (:order_id, :region_name, :order_date, :order_type)";
            param = new DynamicParameters();
            param.Add("order_id", order.Id);
            param.Add("customer_id", order.CustomerId);
            param.Add("region_name", order.RegionName);
            param.Add("order_date", order.OrderDate);
            param.Add("order_type", order.OrderType);
            await using (var connection = GetConnectionBySearchKey(order.RegionName))
            {
                await connection.ExecuteAsync(indexSql2, param);
            }
        }
        
        public async Task UpdateOrderState(long orderId, OrderState orderState, CancellationToken token = default)
        {
            const string sql = @$"
            UPDATE {Table} SET state = :state WHERE id = :order_id;";

            await using (var connection = GetConnectionByShardKey(orderId))
            {
                var param = new DynamicParameters();
                param.Add("order_id", orderId);
                param.Add("state", orderState);

                await connection.ExecuteAsync(sql, param);
            }
        }

        private static IEnumerable<T> SortByColumns<T>(IEnumerable<T> items, SortOrder sortOrder, List<string> sortingFields)
        {
            IOrderedEnumerable<T> sorted;
            if (sortOrder == SortOrder.ASC)
            {
                sorted = items.OrderBy(p => p.GetType().GetProperty(sortingFields.First())?.GetValue(p, null));
                foreach (var sortField in sortingFields.Skip(1))
                {
                    sorted = sorted.ThenBy(p => p.GetType().GetProperty(sortField)?.GetValue(p, null));
                }
            }
            else
            {
                sorted = items.OrderByDescending(p => p.GetType().GetProperty(sortingFields.First())?.GetValue(p, null));
                foreach (var sortField in sortingFields.Skip(1))
                {
                    sorted = sorted.ThenByDescending(p => p.GetType().GetProperty(sortField)?.GetValue(p, null));
                }
            }
            return sorted;
        }
    }
}
