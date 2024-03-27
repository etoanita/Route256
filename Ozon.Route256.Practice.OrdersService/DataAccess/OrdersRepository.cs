using Ozon.Route256.Practice.OrdersService.Exceptions;
using System.Collections.Concurrent;

namespace Ozon.Route256.Practice.OrdersService.DataAccess
{
    public class OrdersRepository : IOrdersRepository
    {
        private static readonly ConcurrentDictionary<long, OrderEntity> OrdersById = new();

        public Task CancelOrderAsync(long orderId, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();

            if (!OrdersById.TryGetValue(orderId, out var order))
                return Task.FromException<OrderEntity>(new NotFoundException($"Order with id={orderId} not found"));

            if (order.State != OrderState.SentToCustomer && order.State != OrderState.Created)
                return Task.FromException<OrderEntity>(new BadRequestException($"Cannot cancel order {orderId}. " +
                    $"Order is in appropriate state."));

            var orderBeforeUpdate = order;
            order = order with { State = OrderState.Cancelled };
            if (OrdersById.TryUpdate(orderId, order, orderBeforeUpdate))
                return Task.CompletedTask;

            return Task.FromException<OrderEntity>(new BadRequestException($"Cannot cancel order {orderId}. Order already cancelled."));
        }

        public Task<OrderState> GetOrderStateAsync(long orderId, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();

            if (!OrdersById.TryGetValue(orderId, out var order))
                return Task.FromException<OrderState>(new NotFoundException($"Order with id={orderId} not found"));

            return Task.FromResult(order.State);
        }

        public Task<IReadOnlyCollection<OrderEntity>> GetOrdersListAsync(List<string> regions, OrderType orderType,
        PaginationParameters pp, SortOrder? sortOrder, List<string> sortingFields, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();

            IEnumerable<OrderEntity> items = OrdersById.Values.Where(x => (!regions.Any() 
                || regions.Contains(x.Region)) && x.OrderType == orderType)
                    .Skip((pp.PageNumber - 1) * pp.PageSize).Take(pp.PageSize);
            if (sortOrder != null) {
                items = SortByColumns(items, sortOrder.Value, sortingFields);
            }

            IReadOnlyCollection<OrderEntity> roResult = items.ToList().AsReadOnly();
            return Task.FromResult(roResult);
        }

        public Task<IReadOnlyCollection<OrderByRegionEntity>> GetOrdersByRegionsAsync(DateTime startDate, List<string> regions, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();

            IEnumerable<OrderEntity> items = OrdersById.Values
                .Where(x => x.OrderDate > startDate && (!regions.Any() || regions.Contains(x.Region)));
            var result = items.GroupBy(x => x.Region).Select(x => new OrderByRegionEntity
            (
                x.Select(x => x.Region).First(), x.Count(), x.Sum(y => y.TotalPrice), 
                x.Sum(y => y.TotalWeight), x.Select(y => y.CustomerId).Distinct().Count())
            );

            IReadOnlyCollection<OrderByRegionEntity> roResult = result.ToList().AsReadOnly();
            return Task.FromResult(roResult);
        }

        public Task<IReadOnlyCollection<OrderEntity>> GetOrdersByClientIdAsync(int clientId, DateTime startFrom, 
            PaginationParameters pp, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();

            var result = OrdersById.Values.
                Where(x => x.CustomerId == clientId && x.OrderDate > startFrom)
                .Skip((pp.PageNumber - 1) * pp.PageSize)
                .Take(pp.PageSize);

            IReadOnlyCollection<OrderEntity> rdOnly = result.ToList().AsReadOnly();
            return Task.FromResult(rdOnly);
        }

        private static IEnumerable<T> SortByColumns<T>(IEnumerable<T> items, SortOrder sortOrder, List<string> sortingFields)
        {
            IOrderedEnumerable<T> sorted;
            if (sortOrder == SortOrder.ASC)
            {
                sorted = items.OrderBy(p => p.GetType().GetProperty(sortingFields.First())?.GetValue(p, null));
                foreach (var sortField in sortingFields.Skip(1))
                {
                    sorted = sorted.ThenBy(p => p.GetType().GetProperty(sortField)?.GetValue(p, null));
                }
            }
            else
            {
                sorted = items.OrderByDescending(p => p.GetType().GetProperty(sortingFields.First())?.GetValue(p, null));
                foreach (var sortField in sortingFields.Skip(1))
                {
                    sorted = sorted.ThenByDescending(p => p.GetType().GetProperty(sortField)?.GetValue(p, null));
                }
            }
            return sorted;
        }
    }
}
