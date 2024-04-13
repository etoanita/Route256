using Dapper;
using Ozon.Route256.Practice.OrdersService.Dal.Common.Shard;
using Ozon.Route256.Practice.OrdersService.Dal.Models;

namespace Ozon.Route256.Practice.OrdersService.DataAccess.Postgres;
public class ShardCustomerDbAccess : BaseShardRepository
{
    private const string Fields = "id, name, surname, address, phone";
    private const string Table = $"{ShardsHelper.BucketPlaceholder}.customers";

    public ShardCustomerDbAccess(IShardPostgresConnectionFactory connectionFactory,
        IShardingRule<long> shardingRule, IShardingRule<string> stringShardingRule) : base(connectionFactory,
        shardingRule, stringShardingRule)
    {
    }

    public async Task<CustomerDal?> Find(int customerId, CancellationToken token = default)
    {
        const string sql = @$"
            select {Fields}
            from {Table}
            where id = :id;
        ";
        await using var connection = GetConnectionByShardKey(customerId);
        var param = new DynamicParameters();
        param.Add("id", customerId);
        var cmd = new CommandDefinition(sql, param, cancellationToken: token);
        return await connection.QueryFirstOrDefaultAsync<CustomerDal>(cmd);
    }

    public async Task<CustomerDal[]> FindMany(List<int> ids, CancellationToken token = default)
    {
        var result = new List<CustomerDal>();
        foreach (var bucketId in AllBuckets)
        {
            const string sql = @$"
                select {Fields}
                from {Table}
                where id in (:ids);
            ";

            await using var connection = GetConnectionByBucket(bucketId, token);
            var param = new DynamicParameters();
            param.Add("ids", ids);
            var customers = await connection.QueryAsync<CustomerDal>(sql, param);
            result.AddRange(customers);
        }
        return result.ToArray();
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
        var param = new DynamicParameters();
        param.Add("id", customerId);
        param.Add("name", customerName);
        param.Add("surname", customerSurname);
        param.Add("address", address);
        param.Add("phone", phone);

        await using (var connection = GetConnectionByShardKey(customerId))
        {
            var cmd = new CommandDefinition(sql, param, cancellationToken: token);
            await connection.ExecuteAsync(cmd);
        }
    }
}

