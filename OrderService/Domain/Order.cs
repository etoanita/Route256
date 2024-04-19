using Ozon.Route256.Practice.OrderService.Domain.Core;

namespace Ozon.Route256.Practice.OrderService.Domain
{
    public sealed class Order : Entity<long>
    {
        public int ItemsCount { get; }
        public double TotalPrice { get; }
        public long TotalWeight { get; }
        public OrderType OrderType { get; }
        public DateTime OrderDate { get; }
        public string Region { get; }
        public OrderState State { get; }
        public int CustomerId { get; }

        public static Order CreateInstance(long id, int itemsCount, double totalPrice, long totalWeight, OrderType orderType, DateTime orderDate, string region, OrderState state, int customerId)
        {
            return new Order(id, itemsCount, totalPrice, totalWeight, orderType, orderDate, region, state, customerId);
        }
        private Order(long id, int itemsCount, double totalPrice, long totalWeight, OrderType orderType, DateTime orderDate, string region, OrderState state, int customerId) : base(id)
        {
            ItemsCount = itemsCount;
            TotalPrice = totalPrice;
            TotalWeight = totalWeight;
            OrderType = orderType;
            OrderDate = orderDate;            
            Region = region;
            State = state;
            CustomerId = customerId;
        }
    };
}
