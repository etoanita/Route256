namespace Ozon.Route256.Practice.OrderService.Dal.Models
{
    public class OrderDal
    {
        public OrderDal()
        {
            
        }
        public OrderDal(long Id,
            int ItemsCount,
            double TotalPrice,
            long TotalWeight,
            OrderType OrderType,
            DateTime OrderDate,
            string RegionName,
            OrderState State,
            int CustomerId)
        {
            this.Id = Id;
            this.ItemsCount = ItemsCount;
            this.TotalPrice = TotalPrice;
            this.TotalWeight = TotalWeight;
            this.OrderType = OrderType;
            this.OrderDate = OrderDate;
            this.RegionName = RegionName;
            this.State = State;
            this.CustomerId = CustomerId;
        }

        public long Id { get; init; }
        public int ItemsCount { get; init; }
        public double TotalPrice { get; init; }
        public long TotalWeight { get; init; }
        public OrderType OrderType { get; init; }
        public DateTime OrderDate { get; init; }
        public string RegionName { get; init; }
        public OrderState State { get; set; }
        public int CustomerId { get; init; }

        public void Deconstruct(out long Id, out int ItemsCount, out double TotalPrice, out long TotalWeight, out OrderType OrderType, out DateTime OrderDate, out string RegionName, out OrderState State, out int CustomerId)
        {
            Id = this.Id;
            ItemsCount = this.ItemsCount;
            TotalPrice = this.TotalPrice;
            TotalWeight = this.TotalWeight;
            OrderType = this.OrderType;
            OrderDate = this.OrderDate;
            RegionName = this.RegionName;
            State = this.State;
            CustomerId = this.CustomerId;
        }
    }
}
