using Ozon.Route256.Practice.OrdersService.Bll;
using Ozon.Route256.Practice.OrdersService.DataAccess.Postgres;
using Ozon.Route256.Practice.OrdersService.Exceptions;
using System.Transactions;

namespace Ozon.Route256.Practice.OrdersService.DataAccess
{
    public class OrdersRepositoryDatabase : IOrdersRepository
    {
        private readonly OrdersDbAccessPg _ordersDbAccess;
        private readonly CustomerDbAccessPg _customerDbAccess;

        public OrdersRepositoryDatabase(
            OrdersDbAccessPg ordersDbAccess,
            CustomerDbAccessPg customerDbAccess)
        {
            _ordersDbAccess = ordersDbAccess;
            _customerDbAccess = customerDbAccess;
        }
        public async Task CancelOrderAsync(long orderId, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();

            var order = await _ordersDbAccess.Find(orderId) ?? throw new NotFoundException($"Order with id={orderId} not found");

            if (order.State != OrderState.SentToCustomer && order.State != OrderState.Created)
                throw new BadRequestException($"Cannot cancel order {orderId}. " +
                    $"Order is in inappropriate state.");

            order = order with { State = OrderState.Cancelled };
            await _ordersDbAccess.UpdateOrderState(order.Id, OrderState.Cancelled, ct);
        }

        public async Task<IReadOnlyCollection<OrderEntity>> GetOrdersByClientIdAsync(int clientId, DateTime startFrom, PaginationParameters pp, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();

            var result = await _ordersDbAccess.FindByCustomerId(clientId, startFrom, pp, ct);
            return result.Select(Converters.ConvertOrder).ToList().AsReadOnly();
        }

        public async Task<IReadOnlyCollection<OrderByRegionEntity>> GetOrdersByRegionsAsync(DateTime startDate, List<string> regions, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();

            var result = await _ordersDbAccess.FindByRegions(regions, startDate, ct);
            return result.Select(Converters.ConvertOrderByRegion).ToList().AsReadOnly();
        }

        public async Task<IReadOnlyCollection<OrderEntity>> GetOrdersListAsync(List<string> regions, OrderType orderType, PaginationParameters pp, SortOrder? sortOrder, List<string> sortingFields, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();

            var result = await _ordersDbAccess.Find(regions, orderType, pp, sortOrder, sortingFields, ct);
            return result.Select(Converters.ConvertOrder).ToList().AsReadOnly();
        }

        public async Task<OrderState> GetOrderStateAsync(long orderId, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();
            return await _ordersDbAccess.GetOrderState(orderId, ct);
        }

        public async Task InsertAsync(OrderEntity orderEntity, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();
            using var ts = new TransactionScope(
            TransactionScopeOption.Required,
            new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadCommitted,
                Timeout = TimeSpan.FromSeconds(5)
            },
            TransactionScopeAsyncFlowOption.Enabled);

            await _customerDbAccess.CreateOrUpdate(orderEntity.CustomerId, orderEntity.CustomerName, orderEntity.CustomerSurname, orderEntity.Address, orderEntity.Phone, ct);
            await _ordersDbAccess.Insert(Converters.ConvertOrder(orderEntity), ct);

            ts.Complete();
        }

        public async Task<bool> IsExistsAsync(long orderId, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();
            var order = await _ordersDbAccess.Find(orderId, ct);
            return order != null;
        }

        public async Task UpdateOrderState(long orderId, OrderState state, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();
            var order = await _ordersDbAccess.Find(orderId, ct);
            bool isExists = order != null;
            if (!isExists )
            {
                throw new Exception($"Order {orderId} was not founf");
            }
            await _ordersDbAccess.UpdateOrderState(orderId, state, ct);
        }
    }
}
