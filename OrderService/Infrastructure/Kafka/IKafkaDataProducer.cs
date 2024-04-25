using Confluent.Kafka;

namespace Ozon.Route256.Practice.OrderService.Infrastructure.Kafka;

public interface IKafkaDataProducer<TKey, TValue>
{
    public IProducer<TKey, TValue> Producer { get; }
}