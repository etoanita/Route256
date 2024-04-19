using Ozon.Route256.Practice.OrdersService.DataAccess;
using Ozon.Route256.Practice.OrdersService.Infrastructure.GrpcServices;
using Ozon.Route256.Practice.OrdersService.Infrastructure.Kafka.Models;

namespace Ozon.Route256.Practice.OrdersService.Handlers.OrderRegistration;

internal class OrderEventHandler : IOrderEventHandler
{
    private readonly IOrdersRepository _orderRepository;

    public OrderEventHandler(IOrdersRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task Handle(Order order, CancellationToken token)
    {
        await _orderRepository.UpdateOrderState(order.OrderId, order.OrderState);
    }
}