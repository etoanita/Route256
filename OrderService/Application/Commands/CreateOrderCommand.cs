using MediatR;

namespace Ozon.Route256.Practice.OrderService.Application.Commands;

public sealed class CreateOrderCommand : IRequest<Unit>
{
    public CreateOrderCommand(Domain.OrderAggregate order)
        => Order = order;

    public Domain.OrderAggregate Order { get; }
}
