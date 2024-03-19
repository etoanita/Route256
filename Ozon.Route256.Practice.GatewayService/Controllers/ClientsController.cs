using Microsoft.AspNetCore.Mvc;
using Ozon.Route256.Practice.GatewayService.GrpcServices;

namespace Ozon.Route256.Practice.GatewayService.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private ICustomerService _customerService;
        public ClientsController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        /// <summary>
        /// Возвращает всех клиентов системы
        /// </summary>
        /// <response code="200">Выполнено успешно</response>
        [HttpGet]
        public async Task<List<CustomerDto>> GetClients()
        {
            return await _customerService.GetClients();
        }
    }
}
