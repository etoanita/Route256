using Npgsql;

namespace Ozon.Route256.Practice.OrdersService.Dal.Common;

public static class PostgresMapping
{
    public static void MapCompositeTypes()
    {
        var mapper = NpgsqlConnection.GlobalTypeMapper;
        mapper.MapEnum<DataAccess.OrderType>("order_type");
        mapper.MapEnum<DataAccess.OrderState>("order_state");
    }
}