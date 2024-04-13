using System.Text;
using Murmur;
using Ozon.Route256.Practice.OrdersService.ClientBalancing;

namespace Ozon.Route256.Practice.OrdersService.Dal.Common.Shard;

public class StringShardingRule: IShardingRule<string>
{
    private readonly IDbStore _dbStore;

    public StringShardingRule(IDbStore dbStore)
    {
        _dbStore = dbStore;
    }

    public int GetBucketId(string shardKey)
    {
        var shardKeyHashCode = GetShardKeyHashCode(shardKey);

        return Math.Abs(shardKeyHashCode) % _dbStore.BucketsCount;
    }

    private static int GetShardKeyHashCode(string shardKey)
    {
        var bytes = Encoding.UTF8.GetBytes(shardKey);

        var murmur = MurmurHash.Create32();
        var hash = murmur.ComputeHash(bytes);

        return BitConverter.ToInt32(hash);
    }
}