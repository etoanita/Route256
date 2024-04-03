using Confluent.Kafka;

namespace Ozon.Route256.Practice.OrdersService.Infrastructure.Kafka;

public interface IKafkaDataProducer<TKey, TValue>
{
    public IProducer<TKey, TValue> Producer { get; }
}