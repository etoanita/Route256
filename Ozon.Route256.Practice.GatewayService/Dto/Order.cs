using System;

namespace Ozon.Route256.Practice.GatewayService.Dto
{
    public class OrderDto
    {
        public long OrderId { get; set; }
        public int ItemsCount { get; set; }
        public int TotalPrice { get; set; }
        public int TotalWeight { get; set; }
        public string OrderType { get; set; }
        public DateTime OrderDate { get; set; }
        public string Region { get; set; }
        public string State { get; set; }
        public string CustomerName { get; set; }
        public string CustomerSurname { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
    }

    public class RegionOrderDto
    {
        public string Region { get; set; }
        public int OrdersCount { get; set; }
        public int TotalPrice { get; set; }
        public int TotalWeight { get; set; }
        public int ClientsCount { get; set; }
    }
}
