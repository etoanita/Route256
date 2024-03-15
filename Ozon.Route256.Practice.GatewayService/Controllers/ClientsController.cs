﻿using Microsoft.AspNetCore.Mvc;
using Ozon.Route256.Practice.GatewayService.Dto;
using Ozon.Route256.Practice.GatewayService.GrpcServices;

namespace Ozon.Route256.Practice.GatewayService.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private IGatewayService _gatewayService;
        public ClientsController(IGatewayService gatewayService)
        {
            _gatewayService = gatewayService;
        }

        /// <summary>
        /// Возвращает всех клиентов системы
        /// </summary>
        /// <response code="200">Выполнено успешно</response>
        [HttpGet]
        public async Task<List<CustomerDto>> GetClients()
        {
            return await _gatewayService.GetClients();
        }
    }
}
