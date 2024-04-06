namespace Ozon.Route256.Practice.OrdersService.Infrastructure.Kafka.Models
{
    public record Order
    (
        long OrderId,
        OrderState OrderState,
        DateTimeOffset ChangedAt
    );
}
