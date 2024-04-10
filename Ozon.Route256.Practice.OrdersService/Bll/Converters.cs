using Ozon.Route256.Practice.OrdersService.Dal.Models;
using Ozon.Route256.Practice.OrdersService.DataAccess;

namespace Ozon.Route256.Practice.OrdersService.Bll
{
    public static class Converters
    {
        public static OrderEntity ConvertOrder(OrderDal order, CustomerDal customer)
        {
            return new OrderEntity(order.Id, order.ItemsCount, order.TotalPrice, order.TotalWeight, order.OrderType,
                order.OrderDate, order.Region, order.State, customer.Id,
                customer.Name, customer.Surname, customer.Address, customer.Phone
            );
        }

        public static OrderDal ConvertOrder(OrderEntity order)
        {
            return new OrderDal(order.OrderId, order.ItemsCount, order.TotalPrice, order.TotalWeight,
                order.OrderType, order.OrderDate, order.Region, order.State, order.CustomerId);
        }

        public static OrderByRegionEntity ConvertOrderByRegion(OrderByRegionDal order)
        {
            return new OrderByRegionEntity(order.Region, order.OrdersCount, order.TotalPrice, order.TotalWeight, order.ClientsCount); ;
        }
    }
}
