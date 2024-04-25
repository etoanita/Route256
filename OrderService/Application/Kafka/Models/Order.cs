using Ozon.Route256.Practice.OrderService.Domain;

namespace Ozon.Route256.Practice.OrderService.Infrastructure.Kafka.Models
{
    public record Order
    (
        long OrderId,
        OrderState OrderState,
        DateTimeOffset ChangedAt
    );
}
