using Grpc.Core;

namespace Ozon.Route256.Practice.OrdersService.DataAccess
{
    internal interface IOrdersRepository
    {
        Task InsertAsync(OrderEntity orderEntity, CancellationToken ct = default);
        Task<bool> IsExistsAsync(long orderId, CancellationToken ct = default);
        public Task CancelOrderAsync(long orderId, CancellationToken ct = default);
        public Task<OrderState> GetOrderStateAsync(long orderId, CancellationToken ct = default);
        public Task<IReadOnlyCollection<OrderEntity>> GetOrdersListAsync(List<string> regions, OrderType orderType,
        PaginationParameters pp, SortOrder? sortOrder, List<string> sortingFields, CancellationToken ct = default);
        public Task<IReadOnlyCollection<OrderByRegionEntity>> GetOrdersByRegionsAsync(DateTime startDate, List<string> regions, CancellationToken ct = default);
        public Task<IReadOnlyCollection<OrderEntity>> GetOrdersByClientIdAsync(int clientId, DateTime startFrom, PaginationParameters pp, CancellationToken ct = default);
    }
}
