using Ozon.Route256.Practice.OrdersService.Infrastructure.Kafka.Models;

namespace Ozon.Route256.Practice.OrdersService.Infrastructure.Kafka.Producers;

public interface IOrderProducer
{
    Task ProduceAsync(IReadOnlyCollection<Order> updatedOrders, CancellationToken token);
}