using Ozon.Route256.Practice.OrderService.Application.Commands;
using Ozon.Route256.Practice.OrderService.Domain;

namespace Ozon.Route256.Practice.OrderService.Application.Mappers;

internal interface ICommandMapper
{
    OrderAggregate MapToDomainOrder(CreateOrderCommand command);
    //Domain.OrderType ToCommand(OrderType orderType);
}
