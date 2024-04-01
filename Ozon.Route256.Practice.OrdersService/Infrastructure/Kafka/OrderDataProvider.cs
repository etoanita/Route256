using Confluent.Kafka;
using Microsoft.Extensions.Options;
using Ozon.Route256.Practice.OrdersService.Configurations;

namespace Ozon.Route256.Practice.OrdersService.Infrastructure.Kafka;

internal class OrderDataProvider : IKafkaDataProvider<long, string>
{
    public OrderDataProvider(IOptions<KafkaConfiguration> kafkaConfigurationOptions, ILogger<OrderDataProvider> logger)
    {
        var config = kafkaConfigurationOptions.Value;
        var consumerConfig = new ConsumerConfig
        {
            GroupId = config.ConsumerGroup,
            BootstrapServers = config.Brokers,
            AutoOffsetReset = AutoOffsetReset.Earliest,
            EnableAutoCommit = false,
        };

        ConsumerBuilder<long, string> consumerBuilder = new(consumerConfig);
        Consumer = consumerBuilder
            .SetErrorHandler((_, error) => { logger.LogError(error.Reason); })
            .SetLogHandler((_, message) => logger.LogInformation(message.Message))
            .Build();

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

    public IConsumer<long, string> Consumer { get; }

    public IProducer<long, string> Producer { get; }
}