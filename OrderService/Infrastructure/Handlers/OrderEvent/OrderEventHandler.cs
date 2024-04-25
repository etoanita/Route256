using Ozon.Route256.Practice.OrderService.Application;
using Ozon.Route256.Practice.OrderService.DataAccess;
using Ozon.Route256.Practice.OrderService.Infrastructure.Kafka.Models;

namespace Ozon.Route256.Practice.OrderService.Handlers.OrderRegistration;

internal class OrderEventHandler : IOrderEventHandler
{
    private readonly IUnitOfWork _orderRepository;

    public OrderEventHandler(IUnitOfWork orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task Handle(Order order, CancellationToken token)
    {
        await _orderRepository.UpdateOrderState(order.OrderId, order.OrderState, token);
    }
}