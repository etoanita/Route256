namespace Ozon.Route256.Practice.OrdersService.Dal.Models
{
    public record RegionDal
    (
        int Id,
        string Name,
        int[] DepotsId
    );
}
