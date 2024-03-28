using Ozon.Route256.Practice.OrdersService.Infrastructure.Kafka;
using Ozon.Route256.Practice.OrdersService.Infrastructure.Kafka.Consumers;

namespace Ozon.Route256.Practice.OrdersService.Infrastructure
{
    public static partial class ServiceCollectionExtensions
    {
        public static IServiceCollection AddKafka(this IServiceCollection collection)
        {
            collection.AddSingleton<IKafkaDataProvider<long, string>, OrderDataProvider>();
            collection.AddHostedService<NewOrderConsumer>();

            return collection;
        }
    }
}