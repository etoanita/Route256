using Ozon.Route256.Practice.OrdersService.DataAccess;
using Ozon.Route256.Practice.OrdersService.Infrastructure.Kafka.Models;

namespace Ozon.Route256.Practice.OrdersService.Bll
{
    public class RegionsRepositoryDatabase : IRegionsRepository
    {
        public Task<IReadOnlyCollection<string>> FindNotPresentedAsync(List<string> regions, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<RegionData> FindRegionAsync(string region, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyCollection<string>> GetRegionsListAsync(CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }
    }
}
