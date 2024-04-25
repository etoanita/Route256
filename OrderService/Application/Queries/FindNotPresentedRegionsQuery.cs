using MediatR;
using Ozon.Route256.Practice.OrderService.Domain;

namespace Ozon.Route256.Practice.OrderService.Application.Queries
{
    public class FindNotPresentedRegionsQuery : IRequest<IReadOnlyCollection<string>>
    {
        public List<string> Regions { get; }
        public FindNotPresentedRegionsQuery(List<string> regions)
        {
            Regions = regions;
        }
    }
}
