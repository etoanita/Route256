using Ozon.Route256.Practice.OrderService.Domain.Core;

namespace Ozon.Route256.Practice.OrderService.Domain
{
    public class OrderInfo : Entity<long>
    {
        public int ItemsCount { get; }
        public double TotalPrice { get; }
        public long TotalWeight { get; }
        public OrderType OrderType { get; }
        public DateTime OrderDate { get; }
        public string Region { get; }
        public OrderState State { get; }
        public Customer Customer { get; }

        public OrderInfo(long id, int itemsCount, double totalPrice, long totalWeight, OrderType orderType, DateTime orderDate, string region, OrderState state, Customer customer) : base(id)
        {
            ItemsCount = itemsCount;
            TotalPrice = totalPrice;
            TotalWeight = totalWeight;
            OrderType = orderType;
            OrderDate = orderDate;
            Region = region;
            State = state;
            Customer = customer;
        }
    }
}
