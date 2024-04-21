using MediatR;
using Ozon.Route256.Practice.OrderService.Domain;

namespace Ozon.Route256.Practice.OrderService.Application.Queries
{
    public class GetOrderByClientIdQuery : IRequest<List<OrderInfo>>
    {
        public int ClientId { get; } 
        public DateTime StartFrom { get; }
        public PaginationParameters PaginationParameters { get; }
        public GetOrderByClientIdQuery(int clientId, DateTime startFrom, PaginationParameters pp)
        {
            ClientId = clientId;
            StartFrom = startFrom;
            PaginationParameters = pp;
        }
    }
}
