using System.Text.Json;
using System.Text.Json.Serialization;
using Confluent.Kafka;
using Ozon.Route256.Practice.OrdersService.Infrastructure.Kafka.Consumers;
using Ozon.Route256.Practice.OrdersService.Infrastructure.Kafka.Models;

namespace Ozon.Route256.Practice.OrdersService.Infrastructure.Kafka.Producers;

internal class OrderProducer : IOrderProducer
{
    private readonly IKafkaDataProducer<long, string> _kafkaDataProvider;
    private readonly ILogger<OrderProducer> _logger;
    private const string TopicName = "new_orders";

    private readonly JsonSerializerOptions _jsonSerializerOptions = new()
    {
        Converters =
        {
            new JsonStringEnumConverter()
        }
    };

    public OrderProducer(IKafkaDataProducer<long, string> kafkaDataProvider, ILogger<OrderProducer> logger)
    {
        _kafkaDataProvider = kafkaDataProvider;
        _logger = logger;
    }

    public async Task ProduceAsync(IReadOnlyCollection<OrderShort> updatedOrders, CancellationToken token)
    {
        await Task.Yield();

        var tasks = new List<Task<DeliveryResult<long, string>>>(updatedOrders.Count);

        foreach (var order in updatedOrders)
        {
            if (token.IsCancellationRequested)
            {
                token.ThrowIfCancellationRequested();
            }

            var key = order.OrderId;
            var value = JsonSerializer.Serialize(order, _jsonSerializerOptions);

            var message = new Message<long, string>
            {
                Key = key,
                Value = value
            };
            _logger.LogInformation("Message {} was added to queue to produce", message.Value);
            var task = _kafkaDataProvider.Producer.ProduceAsync(TopicName, message, token);
            tasks.Add(task);
        }

        await Task.WhenAll(tasks);
    }
}