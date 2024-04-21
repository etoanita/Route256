using MediatR;
using Ozon.Route256.Practice.OrderService.Domain;

namespace Ozon.Route256.Practice.OrderService.Application.Queries
{
    public class GetOrdersByRegionQuery : IRequest<List<OrderByRegion>>
    {
        public DateTime StartDate { get; }
        public List<string> Regions { get; }
        public GetOrdersByRegionQuery(DateTime startDate, List<string> regions)
        {
            StartDate = startDate;
            Regions = regions;
        }
    }
}
