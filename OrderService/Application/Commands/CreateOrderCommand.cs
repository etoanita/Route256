using MediatR;

namespace Ozon.Route256.Practice.OrderService.Application.Commands;

public sealed class CreateOrderCommand : IRequest<Unit>
{
    public CreateOrderCommand(OrderDto order)
        => Order = order;

    public OrderDto Order { get; }
}
