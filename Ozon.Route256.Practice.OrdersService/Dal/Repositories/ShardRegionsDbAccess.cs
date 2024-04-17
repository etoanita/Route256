using Dapper;
using Ozon.Route256.Practice.OrdersService.Dal.Common.Shard;
using Ozon.Route256.Practice.OrdersService.Infrastructure.Kafka.Models;

namespace Ozon.Route256.Practice.OrdersService.DataAccess.Postgres
{
    public class ShardRegionsDbAccess : BaseShardRepository
    {
        public ShardRegionsDbAccess(IShardPostgresConnectionFactory connectionFactory,
            IShardingRule<long> shardingRule, IShardingRule<string>  stringShardingRule)
            : base(connectionFactory, shardingRule, stringShardingRule)
        {
        }
        public async Task<IReadOnlyCollection<string>> FindAll(CancellationToken token = default)
        {
            var result = new List<string>();
            const string sql = @$"select name from {ShardsHelper.BucketPlaceholder}.regions";
            //Note: region and depots data store on all shards
            await using var connection = GetConnectionByBucket(1, token);
            var regions = await connection.QueryAsync<string>(sql);
            result.AddRange(regions);
            return result.ToArray();
        }

        public async Task<RegionData> FindRegionDepots(string region, CancellationToken token = default)
        {
            string sql = @$"select * from bucket_0.depots d join bucket_0.regions r on d.id = 
                any(r.depot_ids) where r.name = '{region}'";
            await using var connection = GetConnectionByBucket(1, token);
            var result = await connection.QueryAsync<Coordinate>(sql);
            return new RegionData(result.ToList());
        }
    }
}
