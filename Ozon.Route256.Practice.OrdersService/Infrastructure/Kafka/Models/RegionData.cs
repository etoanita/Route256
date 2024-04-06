using Ozon.Route256.Practice.OrdersService.DataAccess;

namespace Ozon.Route256.Practice.OrdersService.Infrastructure.Kafka.Models
{
    public record RegionData
    (
        List<Coordinate> Depots
    );
}
