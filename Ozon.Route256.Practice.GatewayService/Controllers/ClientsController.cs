﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        [HttpGet]
        public Task GetClients()
        {
            _gatewayService.GetClients();
            return Task.CompletedTask;
        }
    }
}
