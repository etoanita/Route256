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

        /// <summary>
        /// Отменяет заказ
        /// </summary>
        /// <param name="orderId"> Id заказа</param>
        /// <response code="200">Заказ отменен успешно</response>
        /// <response code="400">Заказ отменить нельзя</response>
        /// <response code="404">Заказ не найден</response>
        [HttpGet]
        public async Task CancelOrder(long orderId)
        {
             await _gatewayService.CancelOrder(orderId);
        }

        /// <summary>
        /// Возвращает статус заказа
        /// </summary>
        /// <param name="orderId"> Id заказа</param>
        /// <response code="200">Выполнено успешно</response>
        /// <response code="404">Заказ не найден</response>
        [HttpGet]
        public async Task<OrderState> GetOrderState(long orderId)
        {
            return await _gatewayService.GetOrderState(orderId);
        }

        /// <summary>
        /// Возвращает список заказов
        /// </summary>
        /// <response code="200">Выполнено успешно</response>
        /// <response code="400">Указанного региона нет в системе</response>
        [HttpGet]
        public async Task<List<OrderDto>> GetOrders([FromQuery] GetOrdersRequestParametersDto parameters)
        {
            return await _gatewayService.GetOrders(parameters);
        }

        /// <summary>
        /// Возвращает список заказов
        /// </summary>
        /// <response code="200">Выполнено успешно</response>
        /// <response code="400">Одного или нескольких регионов нет в системе</response>
        [HttpGet]
        public async Task<List<RegionOrderDto>> GetOrdersByRegion(long startDate, [FromQuery]string[] regions) 
        {
            return await _gatewayService.GetOrdersByRegion(startDate, regions);
        }

        /// <summary>
        /// Возвращает список заказов
        /// </summary>
        /// <response code="200">Выполнено успешно</response>
        /// <response code="400">Клиента с указанным id не существует</response>
        [HttpGet]
        public async Task<List<OrderDto>> GetOrdersByClientId(int userId, long startDate, [FromQuery] PaginationParametersDto paginationParameters)
        {
            return await _gatewayService.GetOrdersByUser(userId, startDate, paginationParameters);
        }
    }
}
