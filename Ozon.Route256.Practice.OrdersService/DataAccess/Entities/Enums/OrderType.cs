namespace Ozon.Route256.Practice.OrdersService.DataAccess
{
    public enum OrderType
    {
        Web = 0,
        Api = 1,
        Mobile = 2,
    }

    public enum OrderTypeDb
    {
        [NpgsqlTypes.PgName("web")]
        web = 0,
        [NpgsqlTypes.PgName("api")]
        api = 1,
        [NpgsqlTypes.PgName("mobile")]
        mobile = 2,
    }
}
