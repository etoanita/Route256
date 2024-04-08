using Confluent.Kafka;
using Ozon.Route256.Practice.OrdersService.Handlers.OrderRegistration;
using System.Text.Json;

namespace Ozon.Route256.Practice.OrdersService.Infrastructure.Kafka.Consumers;

public class NewOrderConsumeHandler : IKafkaConsumeHandler<long, string>
{
    private readonly ILogger<NewOrderConsumeHandler> _logger;
    private readonly IOrderRegistrationHandler _orderRegistrationHandler;
    private readonly Random _random = new();

    public NewOrderConsumeHandler(
        IOrderRegistrationHandler orderRegistrationHandler,
        ILogger<NewOrderConsumeHandler> logger)
    {
        _logger = logger;
        _orderRegistrationHandler = orderRegistrationHandler;
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
                    Region = Enum.GetName((Regions)_random.Next(0, 2))
                }
            }
        };
    }

    public async Task HandleAsync(ConsumeResult<long, string> message, CancellationToken cancellationToken)
    {
        try
        {
            var order = JsonSerializer.Deserialize<Models.NewOrder>(message.Message.Value);
            order = AdjustOrderRegion(order);
            await _orderRegistrationHandler.Handle(order, cancellationToken);
            _logger.LogInformation("New order created, {}", message.Message.Value);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
        }
    }
}