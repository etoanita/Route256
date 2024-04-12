using Microsoft.IdentityModel.Tokens;
using Npgsql;
using Ozon.Route256.Practice.OrdersService.Dal.Common;
using Ozon.Route256.Practice.OrdersService.Dal.Common.Shard;
using Ozon.Route256.Practice.OrdersService.Dal.Models;
using System.Data;

namespace Ozon.Route256.Practice.OrdersService.DataAccess.Postgres
{
    public class ShardOrdersDbAccess : BaseShardRepository
    {
        private const string Fields = "id, items_count, total_price, total_weight, order_type, order_date, region_name, state, customer_id";
        private const string FieldsForInsert = "items_count, total_price, total_weight, order_type, order_date, region_name, state, customer_id";
        private const string Table = "orders";

        ILogger<ShardOrdersDbAccess> _logger;

        public ShardOrdersDbAccess(IShardPostgresConnectionFactory connectionFactory,
            IShardingRule<int> shardingRule,
            ILogger<ShardOrdersDbAccess> logger)
             : base(connectionFactory, shardingRule)
        {
            _logger = logger;
        }
        public async Task<OrderDal?> Find(long id, CancellationToken token = default)
        {
            throw new NotImplementedException();
            //    const string sql = @$"
            //    select {Fields}
            //    from {Table} orders
            //    where id = :id;
            //";

            //    await using var connection = _connectionFactory.GetConnection();
            //    await using var command = new NpgsqlCommand(sql, connection);
            //    command.Parameters.Add("id", id);

            //    await connection.OpenAsync(token);
            //    await using var reader = await command.ExecuteReaderAsync(CommandBehavior.SingleRow, token);

            //    var result = await ReadOrderDal(reader, token);
            //    return result.FirstOrDefault();
        }

        public async Task<IReadOnlyCollection<OrderDal>> FindByCustomerId(int customerId, DateTime startFrom, PaginationParameters pp, CancellationToken token = default)
        {
            throw new NotImplementedException();
            //    const string sql = @$"
            //    select {Fields}
            //    from {Table} orders
            //    where customer_id = :customer_id and order_date > :start_from offset :skip limit :take; 
            //";

            //    await using var connection = _connectionFactory.GetConnection();
            //    await using var command = new NpgsqlCommand(sql, connection);
            //    command.Parameters.Add("customer_id", customerId);
            //    command.Parameters.Add("start_from", startFrom);
            //    command.Parameters.Add("skip", (pp.PageNumber - 1) * pp.PageSize);
            //    command.Parameters.Add("take", pp.PageSize);

            //    await connection.OpenAsync(token);
            //    await using var reader = await command.ExecuteReaderAsync(CommandBehavior.SingleRow, token);

            //    return await ReadOrderDal(reader, token);
        }

        public async Task<IReadOnlyCollection<OrderByRegionDal>> FindByRegions(List<string> regions, DateTime startFrom, CancellationToken token = default)
        {
            throw new NotImplementedException();
            //    string sql = @$"
            //    SELECT region_name, COUNT(*) AS orders_count, SUM(total_price) AS total_price, SUM(total_weight) AS total_weight, COUNT(DISTINCT(customer_id)) as clients_count
            //    FROM {Table}
            //    GROUP BY region_name
            //";

            //    await using var connection = _connectionFactory.GetConnection();
            //    await using var command = new NpgsqlCommand(sql, connection);
            //    await connection.OpenAsync(token);
            //    await using var reader = await command.ExecuteReaderAsync(CommandBehavior.SingleRow, token);

            //    return await ReadOrderByRegionDal(reader, token);
        }

        public async Task<IReadOnlyCollection<OrderDal>> Find(List<string> regions, OrderType orderType, PaginationParameters pp, SortOrder? sortOrder, List<string> sortingFields, CancellationToken token = default)
        {
            throw new NotImplementedException();
            //    sortOrder ??= SortOrder.ASC;
            //    sortingFields = sortingFields.Any() ? sortingFields : new List<string> { "region_name"};
            //    string sql = @$"
            //    select {Fields}
            //    from {Table}
            //    where region_name in (:regions) and order_type = :order_type 
            //    order by {sortingFields.First()} {sortOrder} offset :skip limit :take; 
            //";
            //    _logger.LogInformation(sql);
            //    await using var connection = _connectionFactory.GetConnection();
            //    await using var command = new NpgsqlCommand(sql, connection);
            //    command.Parameters.Add(":regions", regions);
            //    command.Parameters.Add(":order_type", orderType);
            //    command.Parameters.Add(":skip", (pp.PageNumber - 1) * pp.PageSize);
            //    command.Parameters.Add(":take", pp.PageSize);

            //    await connection.OpenAsync(token);
            //    await using var reader = await command.ExecuteReaderAsync(CommandBehavior.SingleRow, token);

            //    return await ReadOrderDal(reader, token);
        }

        public async Task<OrderState> GetOrderState(long id, CancellationToken token = default)
        {
            throw new NotImplementedException();
            //    const string sql = @$"
            //    select state
            //    from {Table} orders
            //    where id = :id;
            //";

            //    await using var connection = _connectionFactory.GetConnection();
            //    await using var command = new NpgsqlCommand(sql, connection);
            //    command.Parameters.Add("id", id);

            //    await connection.OpenAsync(token);
            //    await using var reader = await command.ExecuteReaderAsync(CommandBehavior.SingleRow, token);

            //    var result = await ReadStateDal(reader, token);
            //    return result;
        }

        public async Task Insert(OrderDal order, CancellationToken token = default)
        {
            throw new NotImplementedException();
            //const string sql = @$"
            //insert into {Table} ({FieldsForInsert})
            //values (:items_count, :total_price, :total_weight, :order_type, :order_date, :region_name, :state, :customer_id);";

            //using var connection = _connectionFactory.GetConnection();
            //await using var command = new NpgsqlCommand(sql, connection);
            //command.Parameters.Add("items_count", order.ItemsCount);
            //command.Parameters.Add("total_price", order.TotalPrice);
            //command.Parameters.Add("total_weight", order.TotalWeight);
            //command.Parameters.Add("order_type", order.OrderType);
            //command.Parameters.Add("order_date", order.OrderDate);
            //command.Parameters.Add("region_name", order.Region);
            //command.Parameters.Add("state", order.State);
            //command.Parameters.Add("customer_id", order.CustomerId);

            //await connection.OpenAsync(token);
            //await command.ExecuteNonQueryAsync(token);
        }
        
        public async Task UpdateOrderState(long orderId, OrderState orderState, CancellationToken token = default)
        {
            //const string sql = @$"
            //UPDATE {Table} SET state = :state WHERE id = :order_id;";

            //using var connection = _connectionFactory.GetConnection();
            //await using var command = new NpgsqlCommand(sql, connection);
            //command.Parameters.Add("order_id", orderId);
            //command.Parameters.Add("state", orderState);

            //await connection.OpenAsync(token);
            //await command.ExecuteNonQueryAsync(token);
        }


        private static async Task<OrderDal[]> ReadOrderDal(NpgsqlDataReader reader, CancellationToken token)
        {
            var result = new List<OrderDal>();
            while (await reader.ReadAsync(token))
            {
                result.Add(
                    new OrderDal(
                        Id: reader.GetFieldValue<long>(0),
                        ItemsCount: reader.GetFieldValue<int>(1),
                        TotalPrice: reader.GetFieldValue<double>(2),
                        TotalWeight: reader.GetFieldValue<long>(3),
                        OrderType: reader.GetFieldValue<OrderType>(4),
                        OrderDate: reader.GetFieldValue<DateTime>(5),
                        Region: reader.GetFieldValue<string>(6),
                        State: reader.GetFieldValue<OrderState>(7),
                        CustomerId: reader.GetFieldValue<int>(8)
                    ));
            }

            return result.ToArray();
        }

        private static async Task<OrderByRegionDal[]> ReadOrderByRegionDal(NpgsqlDataReader reader, CancellationToken token)
        {
            var result = new List<OrderByRegionDal>();
            while (await reader.ReadAsync(token))
            {
                result.Add(
                    new OrderByRegionDal(
                        Region: reader.GetFieldValue<string>(0),
                        OrdersCount: reader.GetFieldValue<int>(1),
                        TotalPrice: reader.GetFieldValue<double>(2),
                        TotalWeight: reader.GetFieldValue<long>(3),
                        ClientsCount: reader.GetFieldValue<int>(4)                    
                ));
            }

            return result.ToArray();
        }

        private static async Task<OrderState> ReadStateDal(NpgsqlDataReader reader, CancellationToken token)
        {
            OrderState result = OrderState.Created;
            while (await reader.ReadAsync(token))
            {
                result = reader.GetFieldValue<OrderState>(0);
            }

            return result;
        }
    }
}
