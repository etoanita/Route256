using Grpc.Core;
using Ozon.Route256.Practice.LogisticsSimulator.Grpc;
using Ozon.Route256.Practice.OrderService.Application;
using Ozon.Route256.Practice.OrderService.Exceptions;
using static Ozon.Route256.Practice.LogisticsSimulator.Grpc.LogisticsSimulatorService;

namespace Ozon.Route256.Practice.OrderService.Infrastructure.GrpcServices
{
    public sealed class OrdersService : Orders.OrdersBase
    {
        private readonly IOrderService _orderService;
        private readonly IRegionService _regionService;
        private readonly LogisticsSimulatorServiceClient _logisticsSimulatorServiceClient;
        public OrdersService(IOrderService orderService, IRegionService regionService, LogisticsSimulatorServiceClient logisticsSimulatorServiceClient)
        {
            _orderService = orderService;
            _regionService = regionService;
            _logisticsSimulatorServiceClient = logisticsSimulatorServiceClient;
        }

        public override async Task<CreateOrderResponse> CreateOrder(CreateOrderRequest request, ServerCallContext context)
        {
            await _orderService.CreateOrder(request, context.CancellationToken);
            return new();
        }
        public override async Task<CancelOrderResponse> CancelOrder(CancelOrderRequest request, ServerCallContext context)
        {
            try
            {
                var result = await _logisticsSimulatorServiceClient.OrderCancelAsync(new Order { Id = request.OrderId });
                if (!result.Success)
                {
                    if (result.Error.Contains("not found")) //todo: handle correctly
                        throw new RpcException(new Status(StatusCode.NotFound, result.Error));
                    return new CancelOrderResponse { Success = false, Message = result.Error };
                }
                await _orderService.CancelOrder(request.OrderId, context.CancellationToken);
                CancelOrderResponse response = new()
                {
                    Success = true
                };
                return response;
            }
            catch (NotFoundException ex)
            {
                throw new RpcException(new Status(StatusCode.NotFound, ex.Message));
            }
            catch (BadRequestException ex)
            {
                CancelOrderResponse response = new()
                {
                    Success = false,
                    Message = ex.Message
                };
                return response;
            }
        }


        public override async Task<GetOrderStateResponse> GetOrderState(GetOrderStateRequest request, ServerCallContext context)
        {
            try
            {
                var state = await _orderService.GetOrderState(request.OrderId, context.CancellationToken);
                return new GetOrderStateResponse { State = state };
            }
            catch (NotFoundException ex)
            {
                throw new RpcException(new Status(StatusCode.NotFound, ex.Message));
            }
        }

        public override async Task<GetRegionsListResponse> GetRegionsList(GetRegionsListRequest request, ServerCallContext context)
        {
            var regions = await _regionService.GetRegionsList(context.CancellationToken);
            GetRegionsListResponse result = new();
            result.Regions.Add(regions);
            return result;
        }

        public override async Task<GetOrdersListResponse> GetOrdersList(GetOrdersListRequest request, ServerCallContext context)
        {
            var regions = await _regionService.FindNotPresented(request.Regions.ToList());
            if (regions.Any())
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, $"Followed region(s) was not found: {string.Join(',', regions)}"));
            }
            var result = await _orderService.GetOrders(request, context.CancellationToken);
            var items = new GetOrdersListResponse();
            items.OrderItem.Add(result);
            return items;
        }

        public override async Task<GetOrdersByRegionsResponse> GetOrdersByRegions(GetOrdersByRegionsRequest request, ServerCallContext context)
        {
            var regions = await _regionService.FindNotPresented(request.Regions.ToList());
            if (regions.Any())
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, $"Followed region(s) was not found: {string.Join(',', regions)}"));
            }
            var orders = await _orderService.GetOrdersByRegions(request, context.CancellationToken);
            var result = new GetOrdersByRegionsResponse();
            result.OrderItems.AddRange(orders);
            return result;
        }

        public override async Task<GetOrdersByClientIdResponse> GetOrdersByClientId(GetOrdersByClientIdRequest request, ServerCallContext context)
        {
            var orders = await _orderService.GetOrdersByClientId(request, context.CancellationToken);
            var result = new GetOrdersByClientIdResponse();
            result.OrderItems.Add(orders);
            return result;
        }
    }
}
