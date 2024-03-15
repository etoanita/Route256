﻿using Ozon.Route256.Practice.GatewayService.Dto;
using static Ozon.Route256.Practice.GatewayService.Controllers.OrdersController;

namespace Ozon.Route256.Practice.GatewayService.GrpcServices
{
    public interface IGatewayService
    {
        Task CancelOrder(long orderId);
        Task<string> GetOrderState(long orderId);
        Task<List<OrderDto>> GetOrders(GetOrdersRequestParametersDto requestParameters);
        Task<List<RegionOrderDto>> GetOrdersByRegion(DateTime startDate, List<string> regions);
        Task<List<OrderDto>> GetOrdersByUser(int userId, DateTime startDate, PaginationParametersDto paginationParameters);
        Task<List<CustomerDto>> GetClients();
        Task<List<string>> GetRegions();
    }
}
