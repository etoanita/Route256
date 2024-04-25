using Ozon.Route256.Practice.OrderService.Infrastructure.Kafka.Models;

namespace Ozon.Route256.Practice.OrderService.Handlers.OrderRegistration;

public interface IOrderEventHandler
{
    Task Handle(Order order, CancellationToken cancellationToken);
}