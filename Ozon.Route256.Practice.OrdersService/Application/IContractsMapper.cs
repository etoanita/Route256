using Ozon.Route256.Practice.OrderService.Application.Commands;
using Ozon.Route256.Practice.OrderService.Domain;

namespace Ozon.Route256.Practice.OrderService.Application;

internal interface IContractsMapper
{
    OrderDto ToCommand(CreateOrderRequest requestCustomer);
    Domain.OrderType ToCommand(OrderType requestCustomer);
    Domain.SortOrder ToCommand(SortOrder requestCustomer);
    Domain.PaginationParameters ToCommand(PaginationParameters requestCustomer);
    OrderState ToContracts(Domain.OrderState orderState);
    RegionOrderItem ToContracts(Domain.OrderByRegion orderByRegion);
    OrderItem ToContracts(OrderInfo customerInfo);
}
