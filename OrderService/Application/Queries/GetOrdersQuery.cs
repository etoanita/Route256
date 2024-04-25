using MediatR;
using Ozon.Route256.Practice.OrderService.Domain;

namespace Ozon.Route256.Practice.OrderService.Application.Queries;

public sealed class GetOrdersQuery : IRequest<IReadOnlyCollection<OrderInfo>>
{
    public List<string> Regions { get; }
    public OrderType OrderType { get; }
    public PaginationParameters PaginationParameters { get; }
    public SortOrder? SortOrder { get; }
    public List<string> SortingFields { get; }

    public GetOrdersQuery(List<string> regions, OrderType orderType, PaginationParameters pp, SortOrder? sortOrder, List<string> sortingFields)
    {
        Regions = regions;
        OrderType = orderType;
        PaginationParameters = pp;
        SortOrder = sortOrder;
        SortingFields = sortingFields;
    }
}
