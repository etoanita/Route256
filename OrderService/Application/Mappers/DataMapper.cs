using Ozon.Route256.Practice.OrderService.Application.Commands;
using Ozon.Route256.Practice.OrderService.Domain;

namespace Ozon.Route256.Practice.OrderService.Application.Mappers;

internal class DataMapper : ICommandMapper
{
    public OrderAggregate MapToDomainOrder(CreateOrderCommand command)
    {
        var order = OrderAggregate.CreateInstance(
            order: Order.CreateInstance(
                id: command.Order.Id,
                itemsCount: command.Order.ItemsCount,
                totalPrice: command.Order.TotalPrice,
                totalWeight: command.Order.TotalWeight,
                orderType: command.Order.OrderType,
                orderDate: command.Order.OrderDate,
                region: command.Order.RegionName,
                state: command.Order.State,
                customerId: command.Order.CustomerId),
            customer: Customer.CreateInstance(
                id: command.Order.CustomerId,
                firstName: command.Order.CustomerName,
                lastName: command.Order.CustomerSurname,
                address: command.Order.Address,
                phone: command.Order.Phone));
        return order;
    }
}
