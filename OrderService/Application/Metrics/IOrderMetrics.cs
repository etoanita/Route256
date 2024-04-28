using Ozon.Route256.Practice.OrderService.Domain;

namespace Ozon.Route256.Practice.OrderService.Application.Metrics;

public interface IOrderMetrics
{
    void OrderCreated(OrderType type);
}