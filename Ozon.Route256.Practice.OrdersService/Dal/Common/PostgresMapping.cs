using Dapper;
using Npgsql;
using NpgsqlTypes;
using Ozon.Route256.Practice.OrdersService.DataAccess;
using Ozon.Route256.Practice.OrdersService.Infrastructure.Kafka.Models;
using System.Data;

namespace Ozon.Route256.Practice.OrdersService.Dal.Common;

public static class PostgresMapping
{
    public static void MapCompositeTypes()
    {
        DefaultTypeMap.MatchNamesWithUnderscores = true;
        var mapper = NpgsqlConnection.GlobalTypeMapper;
        Dapper.SqlMapper.AddTypeHandler(typeof(OrderTypeDb), new CountryHandler());

    }

    public struct OrderTypeDb
    {
        string value;

        public static OrderTypeDb Web => "web";
        public static OrderTypeDb Api => "api";
        public static OrderTypeDb Mobile => "mobile";

        private OrderTypeDb(string value)
        {
            this.value = value;
        }

        public static implicit operator OrderTypeDb(string value)
        {
            return new OrderTypeDb(value);
        }

        public static implicit operator string(OrderTypeDb country)
        {
            return country.value;
        }
    }

    public class CountryHandler : SqlMapper.ITypeHandler
    {
        public object Parse(Type destinationType, object value)
        {
            if (destinationType == typeof(OrderTypeDb))
                return (OrderTypeDb)((string)value);
            else return null;
        }

        public void SetValue(IDbDataParameter parameter, object value)
        {
            parameter.DbType = DbType.String;
            parameter.Value = (string)((dynamic)value);
        }
    }
}