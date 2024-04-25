using Ozon.Route256.Practice.OrderService.Infrastructure.Kafka;
using Ozon.Route256.Practice.OrderService.Infrastructure.Kafka.Consumers;
using Ozon.Route256.Practice.OrderService.Infrastructure.Kafka.Producers;

namespace Ozon.Route256.Practice.OrderService.Infrastructure
{
    public static partial class ServiceCollectionExtensions
    {
        public static IServiceCollection AddKafka(this IServiceCollection collection)
        {
            collection.AddSingleton<IKafkaDataProducer<long, string>, OrderProducerDataProvider>();
            collection.AddSingleton<IKafkaDataConsumer<long, string>, OrderConsumerDataProvider>();
            collection.AddSingleton<IOrderProducer, OrderProducer>();
            collection.AddScoped<OrderConsumeHandler>();
            collection.AddScoped<NewOrderConsumeHandler>();
            collection.AddHostedService<OrderConsumerService>();
            return collection;
        }
    }
}