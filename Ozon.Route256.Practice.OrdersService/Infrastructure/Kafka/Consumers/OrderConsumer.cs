using Confluent.Kafka;
using Ozon.Route256.Practice.OrdersService.Handlers.OrderRegistration;
using System.Text.Json;

namespace Ozon.Route256.Practice.OrdersService.Infrastructure.Kafka.Consumers
{
    public class OrderConsumer : ConsumerBackgroundService<long, string>
    {
        private readonly ILogger<OrderConsumer> _logger;
        private readonly IOrderEventHandler _orderEventHandler;
        private const string TopicName = "orders_events";

        public OrderConsumer(
            IServiceProvider serviceProvider,
            IKafkaDataProvider<long, string> kafkaDataProvider,
            ILogger<OrderConsumer> logger)
            : base(TopicName, serviceProvider, kafkaDataProvider, logger)
        {
            _logger = logger;
            _orderEventHandler = _scope.ServiceProvider.GetRequiredService<IOrderEventHandler>();
        }

        protected override async Task HandleAsync(ConsumeResult<long, string> message, CancellationToken cancellationToken)
        {
            var order = JsonSerializer.Deserialize<Models.Order>(message.Message.Value);
            _logger.LogInformation("Update order state {} for logger {}", order.OrderId, order.OrderState);
            await _orderEventHandler.Handle(order, cancellationToken);
        }
    }
}
