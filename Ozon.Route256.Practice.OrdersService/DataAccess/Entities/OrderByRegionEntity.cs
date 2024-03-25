namespace Ozon.Route256.Practice.OrdersService.DataAccess
{
    public record OrderByRegionEntity(
        string Region,
        int OrdersCount,
        int TotalPrice,
        int TotalWeight,
        int ClientsCount    
    );
}
