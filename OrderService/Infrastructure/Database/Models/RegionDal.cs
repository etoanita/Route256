namespace Ozon.Route256.Practice.OrderService.Dal.Models
{
    public record RegionDal
    (
        int Id,
        string Name,
        int[] DepotsId
    );
}
