using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        public Task CancelOrder(long orderId)
        {
            _gatewayService.CancelOrder(orderId);
            return Task.CompletedTask;
        }

        [HttpGet]
        public Task<string> GetOrderStatus(long orderId)
        {
            _gatewayService.GetOrderStatus(orderId);
            return Task.FromResult("");
        }

        [HttpGet]
        public Task Get(List<string> regions, int orderType, int start, int end)
        {
            _gatewayService.GetOrders(); ;
            return Task.CompletedTask;
        }

        [HttpGet]
        public Task GetOrdersByRegion(string region) 
        {
            _gatewayService.GetOrdersByRegion(region);
            return Task.CompletedTask;
        }

        [HttpGet]
        public Task GetOrdersByClientId(int clientId)
        {
            _gatewayService.GetOrdersByUser(clientId);
            return Task.CompletedTask;
        }

    }
}
