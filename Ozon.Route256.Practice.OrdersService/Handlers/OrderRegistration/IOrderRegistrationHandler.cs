using Ozon.Route256.Practice.OrdersService.Infrastructure.Kafka.Models;

namespace Ozon.Route256.Practice.OrdersService.Handlers.OrderRegistration;

public interface IOrderRegistrationHandler : IHandler<NewOrder>
{
    Task Handle(NewOrder order, CancellationToken cancellationToken);
}