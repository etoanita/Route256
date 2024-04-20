 using MediatR;
using Ozon.Route256.Practice.OrderService.Domain;

namespace Ozon.Route256.Practice.OrderService.Application.Queries;

internal sealed class GetOrdersQueryHandler : IRequestHandler<GetOrdersQuery, List<OrderInfo>>
{
    private readonly IOrderReadRepository _repository;

    public GetOrdersQueryHandler(IOrderReadRepository repository)
    {
        _repository = repository;
    }

    public Task<List<OrderInfo>> Handle(GetOrdersQuery request, CancellationToken cancellationToken) 
        => _repository.GetOrders(request, cancellationToken);

}
