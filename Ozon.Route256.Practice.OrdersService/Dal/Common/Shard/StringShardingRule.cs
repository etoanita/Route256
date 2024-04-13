using System.Text;
using Murmur;
using Ozon.Route256.Practice.OrdersService.ClientBalancing;

namespace Ozon.Route256.Practice.OrdersService.Dal.Common.Shard;

public class StringShardingRule: IShardingRule<string>
{
    private readonly int _bucketCount;

    public StringShardingRule(IDbStore dbStore)
    {
        _bucketCount = dbStore.BucketsCount;
    }

    public int GetBucketId(string shardKey)
    {
        var shardKeyHashCode = GetShardKeyHashCode(shardKey);

        return Math.Abs(shardKeyHashCode) % _bucketCount;
    }

    private static int GetShardKeyHashCode(string shardKey)
    {
        var bytes = Encoding.UTF8.GetBytes(shardKey);

        var murmur = MurmurHash.Create32();
        var hash = murmur.ComputeHash(bytes);

        return BitConverter.ToInt32(hash);
    }
}