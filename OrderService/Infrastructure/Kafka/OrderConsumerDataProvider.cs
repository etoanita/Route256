using Confluent.Kafka;
using Microsoft.Extensions.Options;
using Ozon.Route256.Practice.OrderService.Configurations;

namespace Ozon.Route256.Practice.OrderService.Infrastructure.Kafka;

internal class OrderConsumerDataProvider : IKafkaDataConsumer<long, string>
{
    public OrderConsumerDataProvider(IOptions<KafkaConfiguration> kafkaConfigurationOptions, ILogger<OrderConsumerDataProvider> logger)
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
    }

    public IConsumer<long, string> Consumer { get; }
}