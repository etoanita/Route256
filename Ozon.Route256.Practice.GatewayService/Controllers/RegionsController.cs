﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ozon.Route256.Practice.GatewayService.GrpcServices;
using Ozon.Route256.Practice.GatewayService.Infrastructure;

namespace Ozon.Route256.Practice.GatewayService.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private IOrderService _orderService;

        public RegionsController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        /// <summary>
        /// Возвращает список всех регионов
        /// </summary>
        /// <response code="200">Выполнено успешно</response>
        [HttpGet]
        [ProducesResponseType(typeof(List<string>), 200)]
        [ProducesResponseType(typeof(CustomExceptionModel), 500)]
        public async Task<List<string>> GetRegions()
        {
            return await _orderService.GetRegions();
        }
    }
}
