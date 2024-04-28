using Google.Protobuf.WellKnownTypes;
using Ozon.Route256.Practice.OrderService.Application.Commands;
using Ozon.Route256.Practice.OrderService.Dal.Models;
using Ozon.Route256.Practice.OrderService.Domain;
using Ozon.Route256.Practice.OrderService.Infrastructure.Kafka.Models;


namespace Ozon.Route256.Practice.OrderService.Application;

internal class ContractsMapper : IContractsMapper
{
    public Domain.OrderAggregate ToCommand(CreateOrderRequest requestOrder)
        => OrderAggregate.CreateInstance(
            Domain.Order.CreateInstance(
            id: requestOrder.OrderId,
            itemsCount: requestOrder.ItemsCount,
            totalPrice: requestOrder.TotalPrice,
            totalWeight: requestOrder.TotalWeight,
            orderType: ToCommand(requestOrder.OrderType),
            orderDate: requestOrder.OrderDate.ToDateTime(),
            region: requestOrder.Region,
            state: ToCommand(requestOrder.State),
            customerId: requestOrder.CustomerId),
            Domain.Customer.CreateInstance(
            id: requestOrder.CustomerId,
            firstName: requestOrder.CustomerName,
            lastName: requestOrder.CustomerSurname,
            address: requestOrder.Address,
            phone: requestOrder.Phone
         ));

    public OrderService.Domain.OrderType ToCommand(OrderType orderType)
    {
        return orderType switch
        {
            OrderType.Api => OrderService.Domain.OrderType.Api,
            OrderType.Web => OrderService.Domain.OrderType.Web,
            OrderType.Mobile => OrderService.Domain.OrderType.Mobile,
            _ => throw new NotImplementedException(),
        };
    }

    public OrderService.Domain.OrderState ToCommand(OrderState orderType)
    {
        return orderType switch
        {
            OrderState.Delivered => OrderService.Domain.OrderState.Delivered,
            OrderState.Created => OrderService.Domain.OrderState.Created,
            OrderState.SentToCustomer => OrderService.Domain.OrderState.SentToCustomer,
            OrderState.Lost => OrderService.Domain.OrderState.Lost,
            OrderState.Cancelled => OrderService.Domain.OrderState.Cancelled,
            _ => throw new NotImplementedException(),
        };
    }

    public Domain.SortOrder ToCommand(SortOrder sortOrder)
    {
        return sortOrder switch
        {
            SortOrder.Asc => Domain.SortOrder.ASC,
            SortOrder.Desc => Domain.SortOrder.DESC,
            _ => throw new NotImplementedException()
        };
    }

    public Domain.PaginationParameters ToCommand(PaginationParameters pagionationParameters)
    {
        return new Domain.PaginationParameters
        (
            PageNumber: pagionationParameters.PageNumber,
            PageSize: pagionationParameters.PageSize
        );
    }

    public OrderItem ToContracts(OrderInfo orderInfo)
        => new()
        {
            Address = orderInfo.Customer.Address,
            CustomerName = orderInfo.Customer.FirstName,
            CustomerSurname = orderInfo.Customer.LastName,
            ItemsCount = orderInfo.ItemsCount,
            OrderDate = orderInfo.OrderDate.ToUniversalTime().ToTimestamp(),
            OrderId = orderInfo.Id,
            OrderType = ToContracts(orderInfo.OrderType),
            Phone = orderInfo.Customer.Phone,
            Region = orderInfo.Region,
            State = ToContracts(orderInfo.State)
        };

    public OrderType ToContracts(OrderService.Domain.OrderType orderType)
    {
        return orderType switch
        {
            OrderService.Domain.OrderType.Web => OrderType.Web,
            OrderService.Domain.OrderType.Api => OrderType.Api,
            OrderService.Domain.OrderType.Mobile => OrderType.Mobile,
            _ => throw new NotImplementedException(),
        };
    }

    public OrderState ToContracts(OrderService.Domain.OrderState orderState)
    {
        return orderState switch
        {
            OrderService.Domain.OrderState.Created => OrderState.Created,
            OrderService.Domain.OrderState.Delivered => OrderState.Delivered,
            OrderService.Domain.OrderState.SentToCustomer => OrderState.SentToCustomer,
            OrderService.Domain.OrderState.Cancelled => OrderState.Cancelled,
            OrderService.Domain.OrderState.Lost => OrderState.Lost,
            _ => throw new NotImplementedException(),
        };
    }

    public RegionOrderItem ToContracts(OrderByRegion orderByRegion)
    {
        return new RegionOrderItem
        {
            Region = orderByRegion.Region,
            ClientsCount = orderByRegion.ClientsCount,
            OrdersCount = orderByRegion.OrdersCount,
            TotalPrice = orderByRegion.TotalPrice,
            TotalWeight = orderByRegion.TotalWeight
        };
    }
}
