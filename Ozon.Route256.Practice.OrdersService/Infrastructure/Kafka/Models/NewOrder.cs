namespace Ozon.Route256.Practice.OrdersService.Infrastructure.Kafka.Models
{
    public record struct NewOrder(
        long Id,
        int Source,
        Customer Customer,
        List<Good> Goods
    );
}
