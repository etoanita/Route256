using Ozon.Route256.Practice.OrdersService.Infrastructure.Kafka.Models;

namespace Ozon.Route256.Practice.OrdersService.Handlers.OrderRegistration;

public interface IOrderEventHandler
{
    Task Handle(Order order, CancellationToken cancellationToken);
}