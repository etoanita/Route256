using Ozon.Route256.Practice.OrdersService.Infrastructure.Kafka;
using Ozon.Route256.Practice.OrdersService.Infrastructure.Kafka.Consumers;
using Ozon.Route256.Practice.OrdersService.Infrastructure.Kafka.Producers;

namespace Ozon.Route256.Practice.OrdersService.Infrastructure
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