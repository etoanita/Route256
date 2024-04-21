using Ozon.Route256.Practice.OrderService.Domain;
using Ozon.Route256.Practice.OrderService.Dal.Models;

namespace Ozon.Route256.Practice.OrderService.Infrastructure.Database.Mappers;

internal class DataLayerMapper : IDataReadMapper, IDataWriteMapper
{
    public OrderInfo JoinOrderAndCustomer(OrderDal orderDal, CustomerDal customerDal)
    {
        return new OrderInfo(orderDal.Id, orderDal.ItemsCount, orderDal.TotalPrice, orderDal.TotalWeight, 
            ConvertOrderType(orderDal.OrderType),
            orderDal.OrderDate, orderDal.RegionName, ConvertOrderState(orderDal.State), Domain.Customer.CreateInstance(
            customerDal.Id, customerDal.Name, customerDal.Surname, customerDal.Address, customerDal.Phone));
    }

    public OrderDal ToOrderDao(Order order)
    {
        return new OrderDal(order.Id, order.ItemsCount, order.TotalPrice, order.TotalWeight,
        ToOrderTypeDal(order.OrderType), order.OrderDate, order.Region, ToOrderStateDal(order.State), order.CustomerId);
    }

    public OrderService.Dal.Models.OrderType ToOrderTypeDal(Domain.OrderType orderType)
    {
        return orderType switch
        {
            Domain.OrderType.Api => Dal.Models.OrderType.Api,
            Domain.OrderType.Web => Dal.Models.OrderType.Web,
            Domain.OrderType.Mobile => Dal.Models.OrderType.Mobile,
            _ => throw new NotImplementedException(),
        };
    }

    public OrderService.Dal.Models.OrderState ToOrderStateDal(Domain.OrderState orderState)
    {
        return orderState switch
        {
            Domain.OrderState.Created=> Dal.Models.OrderState.Created,
            Domain.OrderState.Delivered => Dal.Models.OrderState.Delivered,
            Domain.OrderState.Cancelled => Dal.Models.OrderState.Cancelled,
            Domain.OrderState.Lost => Dal.Models.OrderState.Lost,
            Domain.OrderState.SentToCustomer => Dal.Models.OrderState.SentToCustomer,
            _ => throw new NotImplementedException(),
        };
    }

    public OrderByRegion ConvertOrderByRegion(OrderByRegionDal orderByRegionDal)
    {
        return OrderByRegion.CreateInstance(orderByRegionDal.Region, orderByRegionDal.OrdersCount, orderByRegionDal.TotalPrice,
            orderByRegionDal.TotalWeight, orderByRegionDal.ClientsCount);
    }

    public Domain.OrderState ConvertOrderState(Dal.Models.OrderState orderState)
    {
        return orderState switch
        {
            Dal.Models.OrderState.Created => Domain.OrderState.Created,
            Dal.Models.OrderState.Delivered => Domain.OrderState.Delivered,
            Dal.Models.OrderState.Cancelled => Domain.OrderState.Cancelled,
            Dal.Models.OrderState.Lost => Domain.OrderState.Lost,
            Dal.Models.OrderState.SentToCustomer => Domain.OrderState.SentToCustomer,
            _ => throw new NotImplementedException(),
        };
    }

    public Domain.OrderType ConvertOrderType(Dal.Models.OrderType orderType)
    {
        return orderType switch
        {
            Dal.Models.OrderType.Api => Domain.OrderType.Api,
            Dal.Models.OrderType.Web => Domain.OrderType.Web,
            Dal.Models.OrderType.Mobile => Domain.OrderType.Mobile,
            _ => throw new NotImplementedException(),
        };
    }

    public Dal.Models.SortOrder ToSortOrderDal(Domain.SortOrder sortOrder)
    {
        return sortOrder switch
        {
            Domain.SortOrder.DESC => Dal.Models.SortOrder.DESC,
            Domain.SortOrder.ASC => Dal.Models.SortOrder.ASC,
            _ => throw new NotImplementedException(),
        };
    }
}