namespace Ozon.Route256.Practice.OrdersService.DataAccess
{
    public interface IRegionsRepository
    {
        Task<IReadOnlyCollection<string>> GetRegionsListAsync(CancellationToken ct = default);
        Task<IReadOnlyCollection<string>> FindNotPresentedAsync(List<string> regions, CancellationToken ct = default);
        Task<RegionData> FindRegionAsync(string region, CancellationToken ct = default);
    }
}
