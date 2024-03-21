namespace Ozon.Route256.Practice.GatewayService
{
    public record RegionOrderDto
    (
        string Region,
        int OrdersCount,
        int TotalPrice,
        int TotalWeight,
        int ClientsCount
    );
}
