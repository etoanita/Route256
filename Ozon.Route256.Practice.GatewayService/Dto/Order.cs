namespace Ozon.Route256.Practice.GatewayService
{
    public record OrderDto
    {
        public long OrderId { get; set; }
        public int ItemsCount { get; set; }
        public int TotalPrice { get; set; }
        public int TotalWeight { get; set; }
        public OrderType OrderType { get; set; }
        public DateTime OrderDate { get; set; }
        public string Region { get; set; }
        public OrderState State { get; set; }
        public string CustomerName { get; set; }
        public string CustomerSurname { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
    }
}
