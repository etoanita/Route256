namespace Ozon.Route256.Practice.OrderService.Dal.Models
{
    public record OrderByRegionDal
    (
        string Region,
        int OrdersCount,
        double TotalPrice,
        long TotalWeight,
        int ClientsCount
    );
}
