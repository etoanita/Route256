using Microsoft.AspNetCore.Mvc;
using Ozon.Route256.Practice.GatewayService.Dto;
using Ozon.Route256.Practice.GatewayService.GrpcServices;

namespace Ozon.Route256.Practice.GatewayService.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IGatewayService _gatewayService;
        public OrdersController(IGatewayService gatewayService)
        {
            _gatewayService = gatewayService;
        }

        [HttpGet]
        [Route("api/[controller]/[action]/{orderId}")]
        public async Task CancelOrder(long orderId)
        {
             await _gatewayService.CancelOrder(orderId);
        }

        [HttpGet]
        public async Task<string> GetOrderState(long orderId)
        {
            return await _gatewayService.GetOrderState(orderId);
        }

        [HttpGet]
        public async Task<List<OrderDto>> GetOrders(GetOrdersRequestParametersDto parameters)
        {
            return await _gatewayService.GetOrders(parameters);
        }

        [HttpGet]
        public async Task<List<RegionOrderDto>> GetOrdersByRegion(DateTime startDate, List<string> regions) 
        {
            return await _gatewayService.GetOrdersByRegion(startDate, regions);
        }

        [HttpGet]
        public async Task<List<OrderDto>> GetOrdersByClientId(int userId, DateTime startDate, PaginationParametersDto paginationParameters)
        {
            return await _gatewayService.GetOrdersByUser(userId, startDate, paginationParameters);
        }
    }
}
