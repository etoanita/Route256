namespace Ozon.Route256.Practice.OrdersService.DataAccess
{
    public class RegionsRepository : IRegionsRepository
    {
        private readonly List<string> _regions;
        public RegionsRepository()
        {
            _regions = Enum.GetNames(typeof(Regions)).ToList();
        }
        public Task<IReadOnlyCollection<string>> GetRegionsListAsync(CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();
            IReadOnlyCollection<string> result = _regions.AsReadOnly();
            return Task.FromResult(result);
        }

        public Task<IReadOnlyCollection<string>> FindNotPresentedAsync(List<string> regions) { 
            List<string> result = new();
            foreach (var region in regions) { 
                if (!_regions.Contains(region))
                {
                    result.Add(region);
                }
            }
            IReadOnlyCollection<string> roResult = result.AsReadOnly();
            return Task.FromResult(roResult); 
        }
    }

    public enum Regions
    {
        Moscow,
        StPetersburg,
        Novosibirsk
    }
}
