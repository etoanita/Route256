namespace Ozon.Route256.Practice.OrderService.Infrastructure.Kafka.Models
{
    public record NewOrder(
        long Id,
        int Source,
        Customer Customer,
        List<Good> Goods
    );
}
