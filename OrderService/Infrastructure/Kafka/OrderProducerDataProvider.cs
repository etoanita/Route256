using Confluent.Kafka;
using Microsoft.Extensions.Options;
using Ozon.Route256.Practice.OrderService.Configurations;

namespace Ozon.Route256.Practice.OrderService.Infrastructure.Kafka;

internal class OrderProducerDataProvider : IKafkaDataProducer<long, string>
{
    public OrderProducerDataProvider(IOptions<KafkaConfiguration> kafkaConfigurationOptions, ILogger<OrderProducerDataProvider> logger)
    {
        var config = kafkaConfigurationOptions.Value;

        var producerConfig = new ProducerConfig
        {
            BootstrapServers = config.Brokers,
        };

        ProducerBuilder<long, string> producerBuilder = new(producerConfig);

        Producer = producerBuilder
            .SetErrorHandler((_, error) => { logger.LogError(error.Reason); })
            .SetLogHandler((_, message) => logger.LogInformation(message.Message))
            .Build();
    }

    public IProducer<long, string> Producer { get; }
}