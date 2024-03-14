using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ozon.Route256.Practice.GatewayService.GrpcServices;

namespace Ozon.Route256.Practice.GatewayService.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private IGatewayService _gatewayService;

        public RegionsController(IGatewayService gatewayService)
        {
            _gatewayService = gatewayService;
        }

        [HttpGet]
        public Task GetRegions()
        {
            _gatewayService.GetRegions();
            return Task.CompletedTask;
        }
    }
}
