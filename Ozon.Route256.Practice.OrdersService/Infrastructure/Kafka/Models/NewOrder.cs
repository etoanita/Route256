namespace Ozon.Route256.Practice.OrdersService.Infrastructure.Kafka.Models
{
    public record NewOrder(
        long Id,
        int Source,
        Customer Customer,
        List<Good> Goods
    );
}
