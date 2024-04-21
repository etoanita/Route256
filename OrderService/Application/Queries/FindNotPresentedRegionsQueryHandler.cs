using MediatR;
using Ozon.Route256.Practice.OrderService.DataAccess;
using Ozon.Route256.Practice.OrderService.Domain;

namespace Ozon.Route256.Practice.OrderService.Application.Queries
{
    public class FindNotPresentedRegionsQueryHandler : IRequestHandler<FindNotPresentedRegionsQuery, IReadOnlyCollection<string>>
    {
        private readonly IRegionsRepository _repository;
        public FindNotPresentedRegionsQueryHandler(IRegionsRepository repository)
        {

            _repository = repository;

        }
        public Task<IReadOnlyCollection<string>> Handle(FindNotPresentedRegionsQuery request, CancellationToken cancellationToken)
            => _repository.FindNotPresentedAsync(request.Regions, cancellationToken);
    }
}
