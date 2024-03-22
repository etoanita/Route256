namespace Ozon.Route256.Practice.OrdersService.DataAccess.Entities
{
    public enum SortOrder
    {
        ASC = 0,
        DESC = 1
    }

    public enum OrderType
    {
        Web = 0,
        Api = 1,
        Mobile = 2,
    }

    public enum OrderState
    {
        Created = 0,
        SentToCustomer = 1,
        Delivered = 2,
        Lost = 3,
        Cancelled = 4,
    }
}
