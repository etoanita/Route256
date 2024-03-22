namespace Ozon.Route256.Practice.OrdersService.DataAccess.Entities
{
    public record OrderEntity
    (
        long OrderId,
        int ItemsCount,
        int TotalPrice,
        int TotalWeight,
        OrderType OrderType,
        DateTime OrderDate,
        string Region,
        OrderState State,
        int CustomerId,
        string CustomerName,
        string CustomerSurname,
        string Address,
        string Phone
    );
}
