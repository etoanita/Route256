using Ozon.Route256.Practice.OrderService.Infrastructure.Kafka.Models;

namespace Ozon.Route256.Practice.OrderService.Handlers.OrderRegistration;

public interface IOrderRegistrationHandler
{
    Task Handle(NewOrder order, CancellationToken cancellationToken);
}