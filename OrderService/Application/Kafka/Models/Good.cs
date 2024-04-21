namespace Ozon.Route256.Practice.OrderService.Infrastructure.Kafka.Models
{
    public record Good(
        long Id,
        string Name,
        int Quantity,
        double Price,
        long Weight
    );
}
