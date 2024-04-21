using Ozon.Route256.Practice.OrderService.Infrastructure.Kafka.Models;

namespace Ozon.Route256.Practice.OrderService.Infrastructure.Kafka.Producers;

public interface IOrderProducer
{
    Task ProduceAsync(IReadOnlyCollection<OrderShort> updatedOrders, CancellationToken token);
}