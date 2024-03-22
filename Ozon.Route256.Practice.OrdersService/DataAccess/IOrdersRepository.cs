﻿using Grpc.Core;
using Ozon.Route256.Practice.OrdersService.DataAccess.Entities;

namespace Ozon.Route256.Practice.OrdersService.DataAccess
{
    public interface IOrdersRepository
    {
        public Task<OrderEntity> CancelOrderAsync(long orderId, CancellationToken ct = default);
        public Task<Entities.OrderState> GetOrderStateAsync(long orderId, CancellationToken ct = default);
        public Task<IReadOnlyCollection<OrderEntity>> GetOrdersListAsync(List<string> regions, Entities.OrderType orderType,
        Entities.PaginationParameters pp, Entities.SortOrder? sortOrder, List<string> sortingFields, CancellationToken ct = default);
        public Task<IReadOnlyCollection<OrderByRegionEntity>> GetOrdersByRegionsAsync(DateTime startDate, List<string> regions, CancellationToken ct = default);
        public Task<IReadOnlyCollection<OrderEntity>> GetOrdersByClientIdAsync(int clientId, DateTime startFrom, Entities.PaginationParameters pp, CancellationToken ct = default);
    }
}
