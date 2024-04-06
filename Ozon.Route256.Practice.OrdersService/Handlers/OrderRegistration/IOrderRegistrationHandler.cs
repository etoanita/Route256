using Ozon.Route256.Practice.OrdersService.Infrastructure.Kafka.Models;

namespace Ozon.Route256.Practice.OrdersService.Handlers.OrderRegistration;

public interface IOrderRegistrationHandler
{
    Task Handle(NewOrder order, CancellationToken cancellationToken);
}