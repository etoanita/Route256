using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Ozon.Route256.Practice.GatewayService.GrpcServices;
using Ozon.Route256.Practice.GatewayService.Infrastructure;
using System.ComponentModel.DataAnnotations;

namespace Ozon.Route256.Practice.GatewayService.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        /// <summary>
        /// Отменяет заказ
        /// </summary>
        /// <param name="orderId"> Id заказа</param>
        /// <response code="200">Заказ отменен успешно</response>
        /// <response code="400">Заказ отменить нельзя</response>
        /// <response code="404">Заказ не найден</response>
        [HttpGet]
        [ProducesResponseType(typeof(void), 200)]
        [ProducesResponseType(typeof(CustomBadRequestModel), 400)]
        [ProducesResponseType(typeof(CustomExceptionModel), 404)]
        [ProducesResponseType(typeof(CustomExceptionModel), 500)]
        public async Task CancelOrder([BindRequired, Range(1, long.MaxValue)] long orderId)
        {
            await _orderService.CancelOrder(orderId);
        }

        /// <summary>
        /// Возвращает статус заказа
        /// </summary>
        /// <param name="orderId"> Id заказа</param>
        /// <response code="200">Выполнено успешно</response>
        /// <response code="404">Заказ не найден</response>
        [HttpGet]
        [ProducesResponseType(typeof(OrderState), 200)]
        [ProducesResponseType(typeof(CustomBadRequestModel), 400)]
        [ProducesResponseType(typeof(CustomExceptionModel), 404)]
        [ProducesResponseType(typeof(CustomExceptionModel), 500)]
        public async Task<OrderState> GetOrderState(long orderId)
        {
            return await _orderService.GetOrderState(orderId);
        }

        /// <summary>
        /// Возвращает список заказов
        /// </summary>
        /// <response code="200">Выполнено успешно</response>
        /// <response code="400">Указанного региона нет в системе</response>
        [ProducesResponseType(typeof(List<OrderDto>), 200)]
        [ProducesResponseType(typeof(CustomBadRequestModel), 400)]
        [ProducesResponseType(typeof(CustomExceptionModel), 500)]
        [HttpGet]
        public async Task<List<OrderDto>> GetOrders([FromQuery] GetOrdersRequestParametersDto parameters)
        {
            return await _orderService.GetOrders(parameters);
        }

        /// <summary>
        /// Возвращает список заказов
        /// </summary>
        /// <response code="200">Выполнено успешно</response>
        /// <response code="400">Одного или нескольких регионов нет в системе</response>
        [ProducesResponseType(typeof(List<RegionOrderDto>), 200)]
        [ProducesResponseType(typeof(CustomBadRequestModel), 400)]
        [ProducesResponseType(typeof(CustomExceptionModel), 500)]
        [HttpGet]
        public async Task<List<RegionOrderDto>> GetOrdersByRegion([BindRequired, Range(1, long.MaxValue)] long startDateTimeStamp, [FromQuery]string[] regions) 
        {
            return await _orderService.GetOrdersByRegion(startDateTimeStamp, regions);
        }

        /// <summary>
        /// Возвращает список заказов
        /// </summary>
        /// <response code="200">Выполнено успешно</response>
        /// <response code="400">Клиента с указанным id не существует</response>
        [ProducesResponseType(typeof(List<OrderDto>), 200)]
        [ProducesResponseType(typeof(CustomBadRequestModel), 400)]
        [ProducesResponseType(typeof(CustomExceptionModel), 500)]
        [HttpGet]
        public async Task<List<OrderDto>> GetOrdersByClientId([BindRequired, Range(1, int.MaxValue)] int userId, [BindRequired, Range(1, long.MaxValue)] long startDate, [FromQuery] PaginationParametersDto paginationParameters)
        {
            return await _orderService.GetOrdersByUser(userId, startDate, paginationParameters);
        }
    }
}
