using Ozon.Route256.Practice.OrderService.DataAccess;

namespace Ozon.Route256.Practice.OrderService.Infrastructure.Kafka.Models
{
    public record RegionData
    (
        List<Coordinate> Depots
    );
}
