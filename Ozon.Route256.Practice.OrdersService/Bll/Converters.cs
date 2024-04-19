using Ozon.Route256.Practice.OrderService.Domain;
using Ozon.Route256.Practice.OrdersService.Dal.Models;

namespace Ozon.Route256.Practice.OrdersService.Bll
{
    public static class Converters
    {
        //long id, int itemsCount, double totalPrice, long totalWeight, OrderType orderType, DateTime orderDate, string region, OrderState state, Customer customer
        public static OrderInfo ConvertOrder(OrderDal order, CustomerDal customer)
        {
            return new OrderInfo(order.Id, order.ItemsCount, order.TotalPrice, order.TotalWeight, (OrderService.Domain.OrderType)(int)order.OrderType, order.OrderDate, order.RegionName,
                (OrderService.Domain.OrderState)(int)order.State, OrderService.Domain.Customer.CreateInstance(order.CustomerId, String.Empty, string.Empty, string.Empty, string.Empty));
              /*return OrderAggregate.CreateInstance(Order.CreateInstance(customer.Id,
                order.ItemsCount, order.TotalPrice, order.TotalWeight, (OrderService.Domain.OrderType)(int)order.OrderType, order.OrderDate, order.RegionName, (OrderService.Domain.OrderState)(int)order.State, order.CustomerId),
                OrderService.Domain.Customer.CreateInstance(customer.Id, customer.Name, customer.Surname, customer.Address, customer.Phone)
            ); */
        }

        public static OrderDal ConvertOrder(Order order)
        {
            return new OrderDal(order.Id, order.ItemsCount, order.TotalPrice, order.TotalWeight,
                (Dal.Models.OrderType)(int)order.OrderType, order.OrderDate, order.Region, (Dal.Models.OrderState)(int)order.State, order.CustomerId);
        }

        public static OrderByRegion ConvertOrderByRegion(OrderByRegionDal order)
        {
            return OrderByRegion.CreateInstance(order.Region, order.OrdersCount, order.TotalPrice, order.TotalWeight, order.ClientsCount); ;
        }
    }
}
