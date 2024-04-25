using MediatR;
using Ozon.Route256.Practice.OrderService.Domain;

namespace Ozon.Route256.Practice.OrderService.Application.Queries;

internal sealed class GetOrdersByClientIdQueryHandler : IRequestHandler<GetOrderByClientIdQuery, IReadOnlyCollection<OrderInfo>>
{
    private readonly IOrderReadRepository _repository;

    public GetOrdersByClientIdQueryHandler(IOrderReadRepository repository)
    {
        _repository = repository;
    }

    public Task<IReadOnlyCollection<OrderInfo>> Handle(GetOrderByClientIdQuery request, CancellationToken cancellationToken)
        => _repository.GetOrdersByClientId(request, cancellationToken);

}