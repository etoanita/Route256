namespace Ozon.Route256.Practice.OrdersService.DataAccess
{
    public record OrderByRegionEntity(
        string Region,
        int OrdersCount,
        double TotalPrice,
        long TotalWeight,
        int ClientsCount    
    );
}
