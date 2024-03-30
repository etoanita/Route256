namespace Ozon.Route256.Practice.OrdersService.DataAccess
{
    public record OrderEntity
    (
        long OrderId,
        int ItemsCount,
        double TotalPrice,
        long TotalWeight,
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
