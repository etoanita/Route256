using Confluent.Kafka;
using Ozon.Route256.Practice.OrdersService.Handlers.OrderRegistration;
using System.Text.Json;

namespace Ozon.Route256.Practice.OrdersService.Infrastructure.Kafka.Consumers;

public class NewOrderConsumer : ConsumerBackgroundService<long, string>
{
    private readonly ILogger<NewOrderConsumer> _logger;
    private readonly IOrderRegistrationHandler _orderRegistrationHandler;
    private const string TopicName = "pre_orders";
    private readonly Random _random = new Random();

    public NewOrderConsumer(
        IServiceProvider serviceProvider,
        IKafkaDataProvider<long, string> kafkaDataProvider,
        ILogger<NewOrderConsumer> logger)
        : base(TopicName, serviceProvider, kafkaDataProvider, logger)
    {
        _logger = logger;
       _orderRegistrationHandler = _scope.ServiceProvider.GetRequiredService<IOrderRegistrationHandler>();
    }

    protected override async Task HandleAsync(ConsumeResult<long, string> message, CancellationToken cancellationToken)
    {
        var order = JsonSerializer.Deserialize<Models.NewOrder>(message.Message.Value);
        order = AdjustOrderRegion(order);
        await _orderRegistrationHandler.Handle(order, cancellationToken);
        _logger.LogInformation("New order created, {}", message.Message.Value);
    }

    private enum Regions
    {
        Moscow,
        StPetersburg,
        Novosibirsk
    }

    Models.NewOrder AdjustOrderRegion(Models.NewOrder order)
    {
        return order with
        {
            Customer = order.Customer with
            {
                Address =  order.Customer.Address with
                {
                    Region = Enum.GetName((Regions)_random.Next(1, 3))
                }
            }
        };
    }
}