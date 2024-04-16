using Dapper;
using Npgsql;

namespace Ozon.Route256.Practice.OrdersService.Dal.Common;

public static class PostgresMapping
{
    public static void MapCompositeTypes()
    {
        DefaultTypeMap.MatchNamesWithUnderscores = true;
        var mapper = NpgsqlConnection.GlobalTypeMapper;
       

    }
}