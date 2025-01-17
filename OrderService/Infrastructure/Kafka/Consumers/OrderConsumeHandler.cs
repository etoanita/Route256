﻿using Confluent.Kafka;
using Ozon.Route256.Practice.OrderService.Handlers.OrderRegistration;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ozon.Route256.Practice.OrderService.Infrastructure.Kafka.Consumers
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
            try
            {
                var order = JsonSerializer.Deserialize<Models.Order>(message.Message.Value, _jsonSerializerOptions);
                _logger.LogInformation("Begin update order state for order {OrderId}. Order state: {OrderState}", order.OrderId, order.OrderState);
                await _orderEventHandler.Handle(order, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }
    }
}
