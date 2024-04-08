using Microsoft.AspNetCore.Connections;
using Npgsql;
using Ozon.Route256.Practice.OrdersService.Dal.Common;
using Ozon.Route256.Practice.OrdersService.Infrastructure.Kafka.Models;

namespace Ozon.Route256.Practice.OrdersService.DataAccess.Postgres;
public class CustomerDbAccessPg
{
    private const string Fields = "id, name, surname, address, phone";
    private const string FieldsForInsert = "name, surname, address, phone";
    private const string Table = "customers";

    private readonly IPostgresConnectionFactory _connectionFactory;

    public CustomerDbAccessPg(IPostgresConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }
    public async Task CreateOrUpdate(int customerId,
        string customerName,
        string customerSurname,
        string address,
        string phone, CancellationToken token)
        {
            string sql = @$"
                insert into 
            {Table} ({Fields}) 
            values (:id, :name, :surname, :address, :phone) on conflict (id) do update
            set name = :name, surname = :surname, address = :address, phone = :phone;
            ";

            await using var connection = _connectionFactory.GetConnection();
            await using var command = new NpgsqlCommand(sql, connection);
            command.Parameters.Add("id", customerId);
            command.Parameters.Add("name", customerName);
            command.Parameters.Add("surname", customerSurname);
            command.Parameters.Add("address", address);
            command.Parameters.Add("phone", phone);

        await connection.OpenAsync(token);
            await command.ExecuteReaderAsync(token);
    }
}

