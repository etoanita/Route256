namespace Ozon.Route256.Practice.OrdersService.Dal.Models
{
    public record OrderDal
    (
        long OrderId,
        int ItemsCount,
        double TotalPrice,
        long TotalWeight,
        OrderType OrderType,
        DateTime OrderDate,
        int RegionId,
        int DepotId,
        DataAccess.OrderState State,
        int CustomerId
    );
}
