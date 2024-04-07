using FluentMigrator;
using Ozon.Route256.Practice.OrdersService.Dal.Common;

namespace Ozon.Route256.Practice.OrdersService.Dal.Migrations
{
    [Migration(2, "Fill regions and depots")]
    public class FillRegionsAndDepots : SqlMigration
    {
        protected override string GetUpSql(IServiceProvider services) => @"
            INSERT INTO regions
            VALUES ('Moscow', '{1}'),
                   ('StPetersburg', '{2}'),
                   ('Novosibirsk', '{3}');

            INSERT INTO depots
            VALUES (1, 55.7558, 37.6173),
                   (2, 59.9311, 30.3609),
                   (3, 54.9833, 82.8964);
        ";

        protected override string GetDownSql(IServiceProvider services) => @"
            truncate regions, depots;
        ";
    }
}
