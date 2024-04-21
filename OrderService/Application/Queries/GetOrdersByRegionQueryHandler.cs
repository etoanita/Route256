using MediatR;
using Ozon.Route256.Practice.OrderService.Domain;

namespace Ozon.Route256.Practice.OrderService.Application.Queries;

internal sealed class GetOrdersByRegionQueryHandler : IRequestHandler<GetOrdersByRegionQuery, IReadOnlyCollection<OrderByRegion>>
{
    private readonly IOrderReadRepository _repository;

    public GetOrdersByRegionQueryHandler(IOrderReadRepository repository)
    {
        _repository = repository;
    }

    public Task<IReadOnlyCollection<OrderByRegion>> Handle(GetOrdersByRegionQuery request, CancellationToken cancellationToken)
        => _repository.GetOrdersByRegions(request, cancellationToken);

}