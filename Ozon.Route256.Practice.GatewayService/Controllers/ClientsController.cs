using Microsoft.AspNetCore.Mvc;
using Ozon.Route256.Practice.GatewayService.GrpcServices;
using Ozon.Route256.Practice.GatewayService.Infrastructure;

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
        [ProducesResponseType(typeof(CustomerDto), 200)]
        [ProducesResponseType(typeof(CustomExceptionModel), 500)]
        public async Task<List<CustomerDto>> GetClients()
        {
            return await _customerService.GetClients();
        }
    }
}
