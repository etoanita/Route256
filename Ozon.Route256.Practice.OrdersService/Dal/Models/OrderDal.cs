namespace Ozon.Route256.Practice.OrdersService.Dal.Models
{
    public record OrderDal
    (
        long Id,
        int ItemsCount,
        double TotalPrice,
        long TotalWeight,
        DataAccess.OrderType OrderType,
        DateTime OrderDate,
        string Region,
        DataAccess.OrderState State,
        int CustomerId
    );
}
