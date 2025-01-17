﻿using FluentMigrator;
using Ozon.Route256.Practice.OrderService.Dal.Common.Shard;
using Ozon.Route256.Practice.OrderService.Dal.Common;

namespace Ozon.Route256.Practice.OrderService.Dal.Migrations;

[Migration(1, "Initial migration")]
public class Initial: ShardSqlMigration
{
protected override string GetUpSql(IServiceProvider services) => @"

--create type order_type AS ENUM ('web', 'api', 'mobile');
--create type order_state AS ENUM ('created', 'sentToCustomer', 'delivered', 'lost', 'cancelled');

create table if not exists orders(
    id int primary key,
    items_count int,
    total_price double precision,
    total_weight bigint,
    order_type int,
    order_date timestamp,
    region_name text,
    state int,
    customer_id int
);

create table if not exists customers(
    id int primary key,
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
    drop table if exists depots;";
}