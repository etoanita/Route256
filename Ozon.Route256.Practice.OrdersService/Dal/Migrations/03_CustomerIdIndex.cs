using FluentMigrator;
using Ozon.Route256.Practice.OrdersService.Dal.Common.Shard;

namespace Ozon.Route256.Practice.CustomerService.Dal.Migrations;


[Migration(3, "Customer id index")]
public class CustomerIdIndex: ShardSqlMigration
{
    protected override string GetUpSql(
        IServiceProvider services) => @"

create table if not exists idx_orders_customer_id
(
    order_id bigint NOT NULL,
    customer_id int  NOT NULL,
    order_date timestamp,
    PRIMARY KEY (order_id)
);

";
    protected override string GetDownSql(
        IServiceProvider services) => @"
    
    drop table if exists idx_orders_customer_id;

";
}