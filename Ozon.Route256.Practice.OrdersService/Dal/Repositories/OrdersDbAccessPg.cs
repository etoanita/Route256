using Npgsql;
using Ozon.Route256.Practice.OrdersService.Dal.Common;
using Ozon.Route256.Practice.OrdersService.Dal.Models;

namespace Ozon.Route256.Practice.OrdersService.DataAccess.Postgres
{
    public class OrdersDbAccessPg
    {
        private readonly IPostgresConnectionFactory _connectionFactory;

        public OrdersDbAccessPg(IPostgresConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }
        public async Task<OrderDal> Find(long orderId, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public async Task<IReadOnlyCollection<OrderDal>> FindByCustomerId(int customerId, DateTime startFomrm, PaginationParameters pp, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public async Task<IReadOnlyCollection<OrderByRegionDal>> FindByRegions(List<string> regions, DateTime startFrom, CancellationToken token = default)
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
            throw new NotImplementedException();
        }
        
        public async Task<bool> IsExists(long orderId, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateOrderState(long orderId, OrderState orderState, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }
    }
}
