using Npgsql;
using Ozon.Route256.Practice.OrdersService.Dal.Common;
using Ozon.Route256.Practice.OrdersService.Dal.Models;
using System.Data;

namespace Ozon.Route256.Practice.OrdersService.DataAccess.Postgres;
public class CustomerDbAccessPg
{
    private const string Fields = "id, name, surname, address, phone";
    private const string Table = "customers";

    private readonly IPostgresConnectionFactory _connectionFactory;

    public CustomerDbAccessPg(IPostgresConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<CustomerDal?> Find(int id, CancellationToken token = default)
    {
        const string sql = @$"
            select {Fields}
            from {Table}
            where id = :id;
        ";

        await using var connection = _connectionFactory.GetConnection();
        await using var command = new NpgsqlCommand(sql, connection);
        command.Parameters.Add("id", id);

        await connection.OpenAsync(token);
        await using var reader = await command.ExecuteReaderAsync(CommandBehavior.SingleRow, token);

        var result = await ReadCustomerDal(reader, token);
        return result.FirstOrDefault();
    }

    public async Task<CustomerDal[]> FindMany(List<int> ids, CancellationToken token = default)
    {
        string sql = @$"
            select {Fields}
            from {Table}
            where id in (:ids);
        ";

        await using var connection = _connectionFactory.GetConnection();
        await using var command = new NpgsqlCommand(sql, connection);
        command.Parameters.Add("ids", String.Join(',', ids));

        await connection.OpenAsync(token);
        await using var reader = await command.ExecuteReaderAsync(CommandBehavior.SingleRow, token);

        var result = await ReadCustomerDal(reader, token);
        return result;
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

    private static async Task<CustomerDal[]> ReadCustomerDal(NpgsqlDataReader reader, CancellationToken token)
    {
        var result = new List<CustomerDal>();
        while (await reader.ReadAsync(token))
        {
            result.Add(
                new CustomerDal(
                    Id: reader.GetFieldValue<int>(0),
                    Name: reader.GetFieldValue<string>(1),
                    Surname: reader.GetFieldValue<string>(2),
                    Address: reader.GetFieldValue<string>(3),
                    Phone: reader.GetFieldValue<string>(4)
                ));
        }

        return result.ToArray();
    }
}

