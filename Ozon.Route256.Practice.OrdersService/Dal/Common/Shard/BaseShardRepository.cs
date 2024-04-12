using Npgsql;
using Ozon.Route256.Practice.OrdersService.Dal.Common.Shard;

namespace Ozon.Route256.Practice.OrdersService.Dal.Common.Shard;

public class BaseShardRepository
{
    private readonly IShardPostgresConnectionFactory _connectionFactory;
    private readonly IShardingRule<int> _shardingRule;

    public BaseShardRepository(
        IShardPostgresConnectionFactory connectionFactory,
        IShardingRule<int> shardingRule)
    {
        _connectionFactory  = connectionFactory;
        _shardingRule       = shardingRule;
    }

    protected ShardNpgsqlConnection GetConnectionByShardKey(
        int shardKey)
    {
        var bucketId = _shardingRule.GetBucketId(shardKey);
        return _connectionFactory.GetConnectionByBucketId(bucketId);
    }
    
    protected ShardNpgsqlConnection GetConnectionByBucket(
        int bucketId,
        CancellationToken token)
    {
        return _connectionFactory.GetConnectionByBucketId(bucketId);
    }

    protected int GetBucketByShardKey(int shardKey) => 
        _shardingRule.GetBucketId(shardKey);
    
    
    protected ShardNpgsqlConnection GetConnectionBySearchKey(string searchKey)
    {
        throw new NotImplementedException();
        /*var bucketId = _stringShardingRule.GetBucketId(searchKey);
        return _connectionFactory.GetConnectionByBucketId(bucketId);*/
    }

    protected IEnumerable<int> AllBuckets => _connectionFactory.GetAllBuckets();
}