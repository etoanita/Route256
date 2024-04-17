using Ozon.Route256.Practice.OrdersService.DataAccess;
using Ozon.Route256.Practice.OrdersService.DataAccess.Postgres;
using Ozon.Route256.Practice.OrdersService.Infrastructure.Kafka.Models;

namespace Ozon.Route256.Practice.OrdersService.Bll
{
    public class RegionsRepositoryDatabase : IRegionsRepository
    {
        private readonly ShardRegionsDbAccess _regionsDbAccess;

        public RegionsRepositoryDatabase(ShardRegionsDbAccess regionsDbAccess)
        {
            _regionsDbAccess = regionsDbAccess;
        }
        public async Task<IReadOnlyCollection<string>> FindNotPresentedAsync(List<string> regions, CancellationToken ct = default)
        {
            var allRegions = await _regionsDbAccess.FindAll();
            List<string> result = new();
            foreach (var region in regions)
            {
                if (!allRegions.Contains(region))
                {
                    result.Add(region);
                }
            }
            IReadOnlyCollection<string> roResult = result.AsReadOnly();
            return roResult;
        }

        public async Task<RegionData> FindRegionAsync(string region, CancellationToken ct = default)
        {
            return await _regionsDbAccess.FindRegionDepots(region, ct);
        }

        public async Task<IReadOnlyCollection<string>> GetRegionsListAsync(CancellationToken ct = default)
        {
            return await _regionsDbAccess.FindAll(ct);
        }
    }
}
