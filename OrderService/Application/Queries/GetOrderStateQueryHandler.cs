using MediatR;
using Ozon.Route256.Practice.OrderService.Domain;

namespace Ozon.Route256.Practice.OrderService.Application.Queries
{
    public class GetOrderStateQueryHandler : IRequestHandler<GetOrderStateQuery, OrderState>
    {
        private readonly IOrderReadRepository _repository;

        public GetOrderStateQueryHandler(IOrderReadRepository repository)
        {
            _repository = repository;
        }
        public Task<OrderState> Handle(GetOrderStateQuery request, CancellationToken cancellationToken) =>
            _repository.GetOrderState(request, cancellationToken);
    }
}
