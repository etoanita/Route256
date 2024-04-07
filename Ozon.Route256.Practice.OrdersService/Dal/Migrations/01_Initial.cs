using FluentMigrator;
using Ozon.Route256.Practice.OrdersService.Dal.Common;

namespace Ozon.Route256.Practice.OrdersService.Dal.Migrations;

[Migration(1, "Initial migration")]
public class Initial: SqlMigration
{
protected override string GetUpSql(IServiceProvider services) => @"

create type order_type AS ENUM ('Web', 'Api', 'Mobile');
create type order_state AS ENUM ('Created', 'SentToCustomer', 'Delivered', 'Lost', 'Cancelled');

create table if not exists orders(
    id bigserial primary key,
    items_count int,
    total_price double precision,
    total_weight bigint,
    order_type order_type,
    order_date timestamp,
    region_name text,
    state order_state,
    customer_id text
);

create table if not exists customers(
    id serial primary key,
    name text,
    surname text,
    address text,
    phone text
);

create table if not exists regions(
    name text,
    depot_ids int[]
);

create table if not exists depots (
    id int,
    latitude double precision,
    longitude double precision
)";

protected override string GetDownSql(
    IServiceProvider services) => @"

    drop table if exists customers;
    drop table if exists orders;
    drop table if exists regions;
    drop table if exists depots;
    drop type order_type;
    drop type order_state";
}