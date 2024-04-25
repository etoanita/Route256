using FluentMigrator;
using Ozon.Route256.Practice.OrderService.Dal.Common.Shard;

namespace Ozon.Route256.Practice.OrderService.Dal.Migrations;


[Migration(4, "Customer's region name index")]
public class RegionNameIndex: ShardSqlMigration
{
    protected override string GetUpSql(
        IServiceProvider services) => @"

create table if not exists idx_orders_region_name
(
    order_id bigint NOT NULL,
    region_name text NOT NULL,
    order_date timestamp NOT NULL,
    order_type int NOT NULL,
    PRIMARY KEY (order_id)
);

";
    protected override string GetDownSql(
        IServiceProvider services) => @"
    
    drop table if exists idx_orders_region_name;

";
}