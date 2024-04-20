using Ozon.Route256.Practice.OrderService.Domain;

namespace Ozon.Route256.Practice.OrderService.Application;

public interface IUnitOfWork
{
    Task SaveOrder(OrderAggregate order, CancellationToken cancellationToken);
    Task CancelOrder(long orderId, CancellationToken cancellationToken);
}
