using MediatR;

namespace Ozon.Route256.Practice.OrderService.Application.Queries
{
    public class GetRegionsQuery : IRequest<List<string>>
    {
    }
}
