using MediatR;
using Ozon.Route256.Practice.OrderService.DataAccess;

namespace Ozon.Route256.Practice.OrderService.Application.Queries
{
    public class GetRegionsQueryHandler : IRequestHandler<GetRegionsQuery, IReadOnlyCollection<string>>
    {
        private readonly IRegionsRepository _repository;
        public GetRegionsQueryHandler(IRegionsRepository repository)
        {
            _repository = repository;
        }

        public Task<IReadOnlyCollection<string>> Handle(GetRegionsQuery request, CancellationToken cancellationToken) =>
            _repository.GetRegionsListAsync(cancellationToken);
    }
}
