using Microsoft.Extensions.Options;
using Ozon.Route256.Practice.OrderService.Configurations;
using Ozon.Route256.Practice.OrderService.Handlers.OrderRegistration;
using System.Text.Json;
using System.Threading.Tasks.Dataflow;

namespace Ozon.Route256.Practice.OrderService.Infrastructure.Kafka.Consumers
{
    public class OrderConsumerService : ConsumerBackgroundService<long, string>
    {
        private readonly IOptions<KafkaConfiguration> _kafkaConfigurationOptions;

        public OrderConsumerService(
            IServiceProvider serviceProvider,
            IKafkaDataConsumer<long, string> kafkaDataProvider,
            IOptions<KafkaConfiguration> kafkaConfigurationOptions,
            ILogger<OrderConsumerService> logger) : base(serviceProvider, kafkaDataProvider, logger)
        {
            _kafkaConfigurationOptions = kafkaConfigurationOptions;
            Consumers = new Dictionary<string, IKafkaConsumeHandler<long, string>>
            {
                { _kafkaConfigurationOptions.Value.Topics.NewOrderTopic, _scope.ServiceProvider.GetRequiredService<NewOrderConsumeHandler>() },
                { _kafkaConfigurationOptions.Value.Topics.OrderTopic, _scope.ServiceProvider.GetRequiredService<OrderConsumeHandler>() },
            };
        }
    }
}
