using Confluent.Kafka;
using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.DataProtection.KeyManagement;

namespace Ozon.Route256.Practice.OrderService.Infrastructure.Kafka
{
    public interface IKafkaDataConsumer<TKey, TValue>
    {
        public IConsumer<TKey, TValue> Consumer { get; }

    }
}
