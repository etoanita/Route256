namespace Ozon.Route256.Practice.OrdersService.DataAccess
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
       // string CustomerName, //todo: get this data from customer service
       // string CustomerSurname,
        string Address,
        string Phone
    );
}
