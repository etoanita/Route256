using Confluent.Kafka;

namespace Ozon.Route256.Practice.OrderService.Infrastructure.Kafka.Consumers
{
    public interface IKafkaConsumeHandler<TKey, TValue>
    {
        Task HandleAsync(ConsumeResult<TKey, TValue> message, CancellationToken cancellationToken);
    }
}
