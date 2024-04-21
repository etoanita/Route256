using Ozon.Route256.Practice.OrderService.Infrastructure.Kafka.Models;

namespace Ozon.Route256.Practice.OrderService.Application;

public interface IRegionService
{
    Task<IReadOnlyCollection<string>> GetRegionsList(CancellationToken ct = default);
    Task<IReadOnlyCollection<string>> FindNotPresented(List<string> regions, CancellationToken ct = default);

}