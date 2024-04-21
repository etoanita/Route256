using Google.Protobuf.WellKnownTypes;
using Ozon.Route256.Practice.GatewayService.Converters;

namespace Ozon.Route256.Practice.GatewayService.GrpcServices
{
    public class OrderSevice : IOrderService
    {
        private readonly Orders.OrdersClient _ordersClient;
        public OrderSevice(Orders.OrdersClient ordersClient)
        {
            _ordersClient = ordersClient;
        }

        public async Task CancelOrder(long orderId)
        {
            await _ordersClient.CancelOrderAsync(new CancelOrderRequest { OrderId = orderId });
        }

        public async Task<OrderState> GetOrderState(long orderId)
        {
            var request = await _ordersClient.GetOrderStateAsync(new GetOrderStateRequest { OrderId = orderId });
            return request.State;
        }

        public async Task<List<OrderDto>> GetOrders(GetOrdersRequestParametersDto requestParameters)
        {
            var result = await _ordersClient.GetOrdersListAsync(OrderConverter.ConvertGetOrdersRequestParameters(requestParameters));
            return result.OrderItem.Select(OrderConverter.ConvertOrderDto).ToList();
        }

        public async Task<List<RegionOrderDto>> GetOrdersByRegion(long startDate, string[] regions)
        {
            var request = new GetOrdersByRegionsRequest
            {
                StartDate = Timestamp.FromDateTimeOffset(new DateTime(startDate, DateTimeKind.Utc))
            };
            request.Regions.AddRange(regions);
            var result = await _ordersClient.GetOrdersByRegionsAsync(request);
            return result.OrderItems.Select(OrderConverter.ConvertRegionOrderDto).ToList();
        }

        public async Task<List<OrderDto>> GetOrdersByUser(int userId, long startDate, PaginationParametersDto paginationParameters)
        {
            var request = new GetOrdersByClientIdRequest
            {
                ClientId = userId,
                StartDate = Timestamp.FromDateTimeOffset(new DateTime(startDate, DateTimeKind.Utc)),
                PaginationParameters = new PaginationParameters
                {
                    PageSize = paginationParameters.PageSize,
                    PageNumber = paginationParameters.PageNumber
                }
            };
            var result = await _ordersClient.GetOrdersByClientIdAsync(request);
            return result.OrderItems.Select(OrderConverter.ConvertOrderDto).ToList();
        }

        public async Task<List<string>> GetRegions()
        {
            var result = await _ordersClient.GetRegionsListAsync(new GetRegionsListRequest());
            return result.Regions.ToList();
        }
    }
}

