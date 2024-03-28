using Confluent.Kafka;


namespace Ozon.Route256.Practice.OrdersService.Infrastructure.Kafka.Consumers;

public class NewOrderConsumer : ConsumerBackgroundService<long, string>
{
    private readonly ILogger<NewOrderConsumer> _logger;
   // private readonly IOrderRegistrationHandler _orderRegistrationHandler;

    public NewOrderConsumer(
        IServiceProvider serviceProvider,
        IKafkaDataProvider<long, string> kafkaDataProvider,
        ILogger<NewOrderConsumer> logger)
        : base(serviceProvider, kafkaDataProvider, logger)
    {
        _logger = logger;
      //  _orderRegistrationHandler = _scope.ServiceProvider.GetRequiredService<IOrderRegistrationHandler>();
    }

    protected override async Task HandleAsync(ConsumeResult<long, string> message, CancellationToken cancellationToken)
    {

        //var order = new Order(message.Message.Key, OrderState.Created, DateTimeOffset.UtcNow);
        //await _orderRegistrationHandler.Handle(order, cancellationToken);
        _logger.LogInformation("New order created, {}", message.Message.Value);
    }
}