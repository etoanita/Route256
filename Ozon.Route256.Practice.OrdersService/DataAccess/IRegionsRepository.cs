﻿using Ozon.Route256.Practice.OrdersService.Infrastructure.Kafka.Models;

namespace Ozon.Route256.Practice.OrdersService.DataAccess
{
    internal interface IRegionsRepository
    {
        Task<IReadOnlyCollection<string>> GetRegionsListAsync(CancellationToken ct = default);
        Task<IReadOnlyCollection<string>> FindNotPresentedAsync(List<string> regions, CancellationToken ct = default);
        Task<RegionData> FindRegionAsync(string region, CancellationToken ct = default);
    }
}