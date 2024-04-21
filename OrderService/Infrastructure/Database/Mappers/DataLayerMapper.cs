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
            Domain.OrderType.Api => OrderService.Dal.Models.OrderType.Api,
            Domain.OrderType.Web => OrderService.Dal.Models.OrderType.Web,
            Domain.OrderType.Mobile => OrderService.Dal.Models.OrderType.Mobile,
            _ => throw new NotImplementedException(),
        };
    }

    public OrderService.Dal.Models.OrderState ToOrderStateDal(Domain.OrderState orderState)
    {
        return orderState switch
        {
            Domain.OrderState.Created=> OrderService.Dal.Models.OrderState.Created,
            Domain.OrderState.Delivered => OrderService.Dal.Models.OrderState.Delivered,
            Domain.OrderState.Cancelled => OrderService.Dal.Models.OrderState.Cancelled,
            Domain.OrderState.Lost => OrderService.Dal.Models.OrderState.Lost,
            Domain.OrderState.SentToCustomer => OrderService.Dal.Models.OrderState.SentToCustomer,
            _ => throw new NotImplementedException(),
        };
    }

    public OrderByRegion ConvertOrderByRegion(OrderByRegionDal orderByRegionDal)
    {
        throw new NotImplementedException();
    }

    public Domain.OrderState ConvertOrderState(Dal.Models.OrderState orderState)
    {
        throw new NotImplementedException();
    }

    public Domain.OrderType ConvertOrderType(Dal.Models.OrderType orderState)
    {
        throw new NotImplementedException();
    }

    public Dal.Models.SortOrder ToSortOrderDal(Domain.SortOrder orderState)
    {
        throw new NotImplementedException();
    }
}