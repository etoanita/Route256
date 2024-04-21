using MediatR;
using Ozon.Route256.Practice.OrderService.Domain;

namespace Ozon.Route256.Practice.OrderService.Application.Queries
{
    public sealed class GetOrderStateQuery : IRequest<OrderState>
    {
    }
}
