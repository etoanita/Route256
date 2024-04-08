using Npgsql;
using Ozon.Route256.Practice.OrdersService.Dal.Common;
using Ozon.Route256.Practice.OrdersService.Dal.Models;
using Ozon.Route256.Practice.OrdersService.Infrastructure.Kafka.Models;
using System.Data;

namespace Ozon.Route256.Practice.OrdersService.DataAccess.Postgres
{
    public class OrdersDbAccessPg
    {
        private const string Fields = "id, items_count, total_price, total_weight, order_type, order_date, region_name, state, customer_id";
        private const string FieldsForInsert = "items_count, total_price, total_weight, order_type, order_date, region_name, state, customer_id";
        private const string Table = "orders";
        private readonly IPostgresConnectionFactory _connectionFactory;

        public OrdersDbAccessPg(IPostgresConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }
        public async Task<OrderDal?> Find(long id, CancellationToken token = default)
        {
            const string sql = @$"
            select {Fields}
            from {Table} orders
            where id = :id;
        ";

            await using var connection = _connectionFactory.GetConnection();
            await using var command = new NpgsqlCommand(sql, connection);
            command.Parameters.Add("id", id);

            await connection.OpenAsync(token);
            await using var reader = await command.ExecuteReaderAsync(CommandBehavior.SingleRow, token);

            var result = await ReadOrderDal(reader, token);
            return result.FirstOrDefault();
        }

        public async Task<IReadOnlyCollection<OrderDal>> FindByCustomerId(int customerId, DateTime startFomrm, PaginationParameters pp, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public async Task<IReadOnlyCollection<OrderByRegionDal>> FindByRegions(List<string> regions, DateTime startFrom, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public async Task<IReadOnlyCollection<OrderDal>> Find(List<string> regions, OrderType orderType, PaginationParameters pp, SortOrder? sortOrder, List<string> sortingFields, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public async Task<OrderDal> Update(OrderDal order, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public async Task<OrderState> GetOrderState(long orderId, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public async Task Insert(OrderDal order, CancellationToken token = default)
        {
            const string sql = @$"
            insert into {Table} ({FieldsForInsert})
            values (:items_count, :total_price, :total_weight, :order_type, :order_date, :region_name, :state, :customer_id);
        ";

            using var connection = _connectionFactory.GetConnection();
            await using var command = new NpgsqlCommand(sql, connection);
            command.Parameters.Add("items_count", order.ItemsCount);
            command.Parameters.Add("total_price", order.TotalPrice);
            command.Parameters.Add("total_weight", order.TotalWeight);
            command.Parameters.Add("order_type", order.OrderType);
            command.Parameters.Add("order_date", order.OrderDate);
            command.Parameters.Add("region_name", order.Region);
            command.Parameters.Add("state", order.State);
            command.Parameters.Add("customer_id", order.CustomerId);

            await connection.OpenAsync(token);
            await command.ExecuteNonQueryAsync(token);
        }
        
        public async Task<bool> IsExists(long orderId, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateOrderState(long orderId, OrderState orderState, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        //private const string Fields = "id, items_count, total_price, total_weight, order_type, order_date, region_name, state, customer_id";
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
    }
}
