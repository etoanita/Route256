using Microsoft.AspNetCore.Connections;
using Npgsql;
using Ozon.Route256.Practice.OrdersService.Dal.Common;
using Ozon.Route256.Practice.OrdersService.Infrastructure.Kafka.Models;
using System.Data;

namespace Ozon.Route256.Practice.OrdersService.DataAccess.Postgres
{
    public class RegionsDbAccessPg
    {
        private readonly IPostgresConnectionFactory _connectionFactory;
        public RegionsDbAccessPg(IPostgresConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }
        public async Task<IReadOnlyCollection<string>> FindAll(CancellationToken token = default)
        {
            const string sql = @$"
            select name
            from regions
        ";

            await using var connection = _connectionFactory.GetConnection();
            await using var command = new NpgsqlCommand(sql, connection);

            await connection.OpenAsync(token);
            await using var reader = await command.ExecuteReaderAsync(CommandBehavior.SingleRow, token);

            var result = new List<string>();
            while (await reader.ReadAsync(token))
            {
                result.Add(reader.GetFieldValue<string>(0));
            }
            return result;
        }

        public Task<RegionData> FindRegion(string region)
        {
            throw new NotImplementedException();
        }
    }
}
