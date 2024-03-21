namespace Ozon.Route256.Practice.GatewayService
{
    public record OrderDto
    (
        long OrderId,
        int ItemsCount,
        int TotalPrice,
        int TotalWeight,
        OrderType OrderType,
        DateTime OrderDate,
        string Region,
        OrderState State,
        string CustomerName,
        string CustomerSurname,
        string Address,
        string Phone
    );
}
