namespace Ozon.Route256.Practice.OrdersService.Infrastructure.Kafka.Models
{
    public record struct Good(
        long Id,
        string Name,
        int Quantity,
        double Price,
        long Weight
    );
}
