using Ozon.Route256.Practice.OrderService.Domain;

namespace Ozon.Route256.Practice.OrderService.Application;

public interface IOrderService
{
    Task CreateOrder(CreateOrderRequest request, CancellationToken cancellationToken);

    Task CancelOrder(long orderId, CancellationToken cancellationToken);

    Task<List<OrderItem>> GetOrders(GetOrdersListRequest request, CancellationToken cancellationToken);

    Task<List<OrderByRegion>> GetOrdersByRegions(GetOrdersByRegionsRequest request, CancellationToken cancellationToken);

    Task<List<OrderItem>> GetOrdersByClientId(GetOrdersByClientIdRequest request, CancellationToken cancellationToken);

    Task<OrderState> GetOrderState(long orderId, CancellationToken cancellationToken);
}