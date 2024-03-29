using System.Text.Json;
using System.Text.Json.Serialization;
using Confluent.Kafka;
using Ozon.Route256.Practice.OrdersService.Infrastructure.Kafka.Models;

namespace Ozon.Route256.Practice.OrdersService.Infrastructure.Kafka.Producers;

internal class OrderProducer : IOrderProducer
{
    private readonly IKafkaDataProvider<long, string> _kafkaDataProvider;

    private readonly JsonSerializerOptions _jsonSerializerOptions = new()
    {
        Converters =
        {
            new JsonStringEnumConverter()
        }
    };

    public OrderProducer(IKafkaDataProvider<long, string> kafkaDataProvider)
    {
        _kafkaDataProvider = kafkaDataProvider;
    }

    public async Task ProduceAsync(IReadOnlyCollection<Order> updatedOrders, CancellationToken token)
    {
        await Task.Yield();

        var tasks = new List<Task<DeliveryResult<long, string>>>(updatedOrders.Count);

        foreach (var order in updatedOrders)
        {
            if (token.IsCancellationRequested)
            {
                token.ThrowIfCancellationRequested();
            }

            var key = order.Id;
            var value = JsonSerializer.Serialize(order, _jsonSerializerOptions);

            var message = new Message<long, string>
            {
                Key = key,
                Value = value
            };
            var task = _kafkaDataProvider.Producer.ProduceAsync("new_orders", message, token);
            tasks.Add(task);
        }

        await Task.WhenAll(tasks);
    }
}