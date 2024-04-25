using Dapper;
using Npgsql;

namespace Ozon.Route256.Practice.OrderService.Dal.Common;

public static class PostgresMapping
{
    public static void MapCompositeTypes()
    {
        DefaultTypeMap.MatchNamesWithUnderscores = true;
    }
}