using Ozon.Route256.Practice.OrderService.Application.Queries;
using Ozon.Route256.Practice.OrderService.Domain;

namespace Ozon.Route256.Practice.OrderService.Application;

public interface IOrderReadRepository
{
    Task<List<OrderInfo>> GetOrders(GetOrdersQuery request, CancellationToken cancellationToken);
    Task<List<OrderInfo>> GetOrdersByClientId(GetOrdersQuery request, CancellationToken cancellationToken);
    Task<List<OrderInfo>> GetOrdersByRegions(GetOrdersQuery request, CancellationToken cancellationToken);
}
