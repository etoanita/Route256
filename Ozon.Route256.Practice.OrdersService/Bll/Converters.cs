using Ozon.Route256.Practice.OrdersService.Dal.Models;
using Ozon.Route256.Practice.OrdersService.DataAccess;

namespace Ozon.Route256.Practice.OrdersService.Bll
{
    public static class Converters
    {
        public static OrderEntity ConvertOrder(OrderDal order)
        {
            return null;
        }

        public static OrderDal ConvertOrder(OrderEntity order)
        {
            return new OrderDal(order.OrderId, order.ItemsCount, order.TotalPrice, order.TotalWeight,
                order.OrderType, order.OrderDate, order.Region, order.State, order.CustomerId);
        }

        public static OrderByRegionEntity ConvertOrderByRegion(OrderByRegionDal order)
        {
            return null;
        }
    }
}
