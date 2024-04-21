using Ozon.Route256.Practice.OrderService.Domain;
using Ozon.Route256.Practice.OrderService.Dal.Models;
using OrderState = Ozon.Route256.Practice.OrderService.Dal.Models.OrderState;
using OrderType = Ozon.Route256.Practice.OrderService.Dal.Models.OrderType;
using SortOrder = Ozon.Route256.Practice.OrderService.Dal.Models.SortOrder;

namespace Ozon.Route256.Practice.OrderService.Infrastructure.Database.Repositories;

internal interface IOrdersRepositoryPg
{
    Task<OrderDal?> Find(long id, CancellationToken token = default);
    Task<IReadOnlyCollection<OrderDal>> Find(List<string> regions, OrderType orderType, PaginationParameters pp, SortOrder? sortOrder, List<string> sortingFields, CancellationToken token = default);
    Task<IReadOnlyCollection<OrderDal>> FindByCustomerId(int customerId, DateTime startFrom, PaginationParameters pp, CancellationToken token = default);
    Task<IReadOnlyCollection<OrderByRegionDal>> FindByRegions(List<string> regions, DateTime startFrom, CancellationToken token = default);
    Task<OrderState> GetOrderState(long id, CancellationToken token = default);
    Task Insert(OrderDal order, CancellationToken token = default);
    Task UpdateOrderState(long orderId, OrderState orderState, CancellationToken token = default);
}