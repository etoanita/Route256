namespace Ozon.Route256.Practice.GatewayService
{
    public record RegionOrderDto
    {
        public string Region { get; set; }
        public int OrdersCount { get; set; }
        public int TotalPrice { get; set; }
        public int TotalWeight { get; set; }
        public int ClientsCount { get; set; }
    }
}
