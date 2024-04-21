
using MediatR;
using Ozon.Route256.Practice.OrderService.Application.Commands;
using Ozon.Route256.Practice.OrderService.Application.Queries;
using System.Threading;

namespace Ozon.Route256.Practice.OrderService.Application
{
    internal sealed class RegionServiceAdapter : IRegionService
    {
        private readonly IMediator _mediator;

        public RegionServiceAdapter(IMediator mediator)
        {
            _mediator = mediator;
        }
        public async Task<IReadOnlyCollection<string>> FindNotPresented(List<string> regions, CancellationToken cancellationToken = default)
        {
            return await _mediator.Send(new FindNotPresentedRegionsQuery(regions), cancellationToken);
        }

        public async Task<IReadOnlyCollection<string>> GetRegionsList(CancellationToken cancellationToken = default)
        {
            return await _mediator.Send(new GetRegionsQuery(), cancellationToken);
        }
    }
}
