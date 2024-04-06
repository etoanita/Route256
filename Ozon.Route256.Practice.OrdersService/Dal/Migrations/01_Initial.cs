using FluentMigrator;
using Ozon.Route256.Practice.OrdersService.Dal.Common;

namespace Ozon.Route256.Practice.OrdersService.Dal.Migrations;

[Migration(1, "Initial migration")]
public class Initial: SqlMigration
{
    protected override string GetUpSql(
        IServiceProvider services) => @"

CREATE TYPE order_type AS ENUM ('Web', 'Api', 'Mobile');
CREATE TYPE order_state AS ENUM ('Created', 'SentToCustomer', 'Delivered', 'Lost', 'Cancelled');

create table orders(
    id bigserial primary key,
    items_count int,
    total_price double precision,
    total_weight bigint,
    order_type order_type,
    order_date timestamp,
    region_id int,
    state order_state,
    customer_id text
);

create table customers(
    id serial primary key,
    name text,
    surname text,
    address text,
    phone text
);

";

    protected override string GetDownSql(
        IServiceProvider services) =>@"

drop table customers;
drop table orders;

";
}