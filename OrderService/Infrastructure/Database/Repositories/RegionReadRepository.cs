using Ozon.Route256.Practice.OrderService.DataAccess;
using Ozon.Route256.Practice.OrderService.DataAccess.Postgres;
using Ozon.Route256.Practice.OrderService.Infrastructure.Kafka.Models;

namespace Ozon.Route256.Practice.OrderService.Bll
{
    public class RegionReadRepository : IRegionsRepository
    {
        private readonly ShardRegionsDbAccess _regionsDbAccess;

        public RegionReadRepository(ShardRegionsDbAccess regionsDbAccess)
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
