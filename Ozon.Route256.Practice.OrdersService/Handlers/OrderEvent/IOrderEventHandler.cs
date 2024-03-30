using Ozon.Route256.Practice.OrdersService.Infrastructure.Kafka.Models;

namespace Ozon.Route256.Practice.OrdersService.Handlers.OrderRegistration;

public interface IOrderEventHandler : IHandler<Order>
{
    Task Handle(Order order, CancellationToken cancellationToken);
}