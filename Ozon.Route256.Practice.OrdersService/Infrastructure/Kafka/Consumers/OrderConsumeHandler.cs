using Confluent.Kafka;
using Ozon.Route256.Practice.OrdersService.Handlers.OrderRegistration;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ozon.Route256.Practice.OrdersService.Infrastructure.Kafka.Consumers
{
    public class OrderConsumeHandler : IKafkaConsumeHandler<long, string>
    {
        private readonly ILogger<OrderConsumeHandler> _logger;
        private readonly IOrderEventHandler _orderEventHandler;
        private readonly JsonSerializerOptions _jsonSerializerOptions = new()
        {
            Converters =
        {
            new JsonStringEnumConverter()
        }
        };

        public OrderConsumeHandler(
            IOrderEventHandler orderEventHandler,
            ILogger<OrderConsumeHandler> logger)
        {
            _logger = logger;
            _orderEventHandler = orderEventHandler;
        }

        public  async Task HandleAsync(ConsumeResult<long, string> message, CancellationToken cancellationToken)
        {
            var order = JsonSerializer.Deserialize<Models.Order>(message.Message.Value, _jsonSerializerOptions);
            _logger.LogInformation("Begin update order state for order {}. Order state: {}", order.OrderId, order.OrderState);
            try
            {
                await _orderEventHandler.Handle(order, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }
    }
}
