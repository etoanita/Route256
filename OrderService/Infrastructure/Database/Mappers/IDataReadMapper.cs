using Ozon.Route256.Practice.OrderService.Domain;
using Ozon.Route256.Practice.OrderService.Dal.Models;

namespace Ozon.Route256.Practice.OrderService.Infrastructure.Database.Mappers;

internal interface IDataReadMapper
{
    OrderInfo JoinOrderAndCustomer(OrderDal orderDal, CustomerDal customerDal);
    OrderByRegion ConvertOrderByRegion(OrderByRegionDal orderByRegionDal);
    Domain.OrderState ConvertOrderState(Dal.Models.OrderState orderState);
}