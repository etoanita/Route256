using Grpc.Core;
using Ozon.Route256.Practice.OrderService.Domain;

namespace Ozon.Route256.Practice.OrdersService.DataAccess
{
    public interface IOrdersRepository
    {
        Task InsertAsync(OrderAggregate orderEntity, CancellationToken ct = default);

        Task CancelOrderAsync(long orderId, CancellationToken ct = default);

        Task UpdateOrderState(long orderId, OrderState state, CancellationToken ct = default);

        Task<bool> IsExistsAsync(long orderId, CancellationToken ct = default);

        Task<OrderState> GetOrderStateAsync(long orderId, CancellationToken ct = default);

        Task<IReadOnlyCollection<OrderInfo>> GetOrdersListAsync(List<string> regions, OrderType orderType,
        PaginationParameters pp, SortOrder? sortOrder, List<string> sortingFields, CancellationToken ct = default);

        Task<IReadOnlyCollection<OrderByRegion>> GetOrdersByRegionsAsync(DateTime startDate, List<string> regions, CancellationToken ct = default);

        Task<IReadOnlyCollection<OrderInfo>> GetOrdersByClientIdAsync(int clientId, DateTime startFrom, PaginationParameters pp, CancellationToken ct = default);
    }
}
