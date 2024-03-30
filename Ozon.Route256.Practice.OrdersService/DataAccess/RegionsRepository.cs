namespace Ozon.Route256.Practice.OrdersService.DataAccess
{
    public class RegionsRepository : IRegionsRepository
    {
        private readonly List<string> _regions;
        private readonly Dictionary<string, RegionData> _regionsStorage;
        public RegionsRepository()
        {
            _regions = Enum.GetNames(typeof(Regions)).ToList();
            _regionsStorage = new Dictionary<string, RegionData>();
            Random rnd = new();
            for (int i = 0; i < _regions.Count; i++)
            {
                const int MAX_VALUE = 90;
                const int MIN_VALUE = -90;
                _regionsStorage.Add(_regions[i], new RegionData()
                {
                    Depots = new List<Coordinate> {
                        new(rnd.NextDouble() * (MAX_VALUE - MIN_VALUE) + MIN_VALUE, rnd.NextDouble() * (MAX_VALUE - MIN_VALUE) + MIN_VALUE)
                    }
                });
            }
        }
        public Task<IReadOnlyCollection<string>> GetRegionsListAsync(CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();
            IReadOnlyCollection<string> result = _regions.AsReadOnly();
            return Task.FromResult(result);
        }

        public Task<RegionData> FindRegionAsync(string region, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();

            return Task.FromResult(_regionsStorage[region]);
        }

        public Task<IReadOnlyCollection<string>> FindNotPresentedAsync(List<string> regions, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();

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

        private enum Regions
        {
            Moscow,
            StPetersburg,
            Novosibirsk
        }
    }

    public record struct RegionData
    (
        List<Coordinate> Depots
    );

    public record struct Coordinate
    (
        double orderLatitude, 
        double orderLongitude
    );
}
