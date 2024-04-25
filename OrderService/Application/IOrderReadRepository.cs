using Ozon.Route256.Practice.OrderService.Application.Queries;
using Ozon.Route256.Practice.OrderService.Domain;

namespace Ozon.Route256.Practice.OrderService.Application;

public interface IOrderReadRepository
{
    Task<bool> IsExistsAsync(long orderId, CancellationToken ct = default);
    Task<IReadOnlyCollection<OrderInfo>> GetOrders(GetOrdersQuery request, CancellationToken cancellationToken);
    Task<OrderState> GetOrderState(GetOrderStateQuery request, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<OrderInfo>> GetOrdersByClientId(GetOrderByClientIdQuery request, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<OrderByRegion>> GetOrdersByRegions(GetOrdersByRegionQuery request, CancellationToken cancellationToken);
}
