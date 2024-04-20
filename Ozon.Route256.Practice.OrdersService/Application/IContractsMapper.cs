using Ozon.Route256.Practice.OrderService.Application.Commands;
using Ozon.Route256.Practice.OrderService.Domain;

namespace Ozon.Route256.Practice.CustomerService.Application;

internal interface IContractsMapper
{
    OrderDto ToCommand(CreateOrderRequest requestCustomer);

    OrderItem ToContracts(OrderInfo customerInfo);

    OrderState ToContracts(OrderService.Domain.OrderState orderState);
}
