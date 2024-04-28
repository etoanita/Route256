using Ozon.Route256.Practice.OrderService.Application.Metrics;
using Prometheus;

namespace Ozon.Route256.Practice.OrderService.Infrastructure.Metrics;

internal class OrderMetrics : IOrderMetrics
{
    private readonly Counter _counter = Prometheus.Metrics.CreateCounter(
        name:"order_service_order_created_by_type",
        help: "Создание заказа по типу",
        "type");
    private readonly Counter _canceledOrderCounter = Prometheus.Metrics.CreateCounter(
        name: "order_service_order_canceled",
        help: "Отмена заказа",
        "city");

    public void OrderCreated(Domain.OrderType type)
        => _counter.WithLabels(type.ToString()).Inc();

    public void OrderCanceled()
    => _canceledOrderCounter.WithLabels().Inc();
}
