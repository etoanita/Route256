namespace Ozon.Route256.Practice.OrdersService.Infrastructure.Kafka.Models
{
    public record struct Customer(
        int Id,
        Address Address
    );

}
