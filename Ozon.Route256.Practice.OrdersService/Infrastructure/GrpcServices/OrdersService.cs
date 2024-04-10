using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Ozon.Route256.Practice.LogisticsSimulator.Grpc;
using Ozon.Route256.Practice.OrdersService.DataAccess;
using Ozon.Route256.Practice.OrdersService.Exceptions;
using static Ozon.Route256.Practice.LogisticsSimulator.Grpc.LogisticsSimulatorService;

namespace Ozon.Route256.Practice.OrdersService.Infrastructure.GrpcServices
{
    public sealed class OrdersService : Orders.OrdersBase
    {
        private readonly IOrdersRepository _ordersRepository;
        private readonly IRegionsRepository _regionsRepository;
        private readonly LogisticsSimulatorServiceClient _logisticsSimulatorServiceClient;
        public OrdersService(IOrdersRepository ordersRepository, IRegionsRepository regionsRepository, LogisticsSimulatorServiceClient logisticsSimulatorServiceClient)
        {
            _ordersRepository = ordersRepository;
            _regionsRepository = regionsRepository;
            _logisticsSimulatorServiceClient = logisticsSimulatorServiceClient;
        }

        public override async Task<CreateOrderResponse> CreateOrder(CreateOrderRequest request, ServerCallContext context)
        {
            await _ordersRepository.InsertAsync(new OrderEntity(0, request.ItemsCount, request.TotalPrice, request.TotalWeight, 
                Converters.ConvertOrderType(request.OrderType), request.OrderDate.ToDateTime(), request.Region, 
                Converters.ConvertOrderState(request.State), request.CustomerId,
                request.CustomerName, request.CustomerSurname, request.Address, request.Phone));
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
                await _ordersRepository.CancelOrderAsync(request.OrderId, context.CancellationToken);
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
                var state = await _ordersRepository.GetOrderStateAsync(request.OrderId, context.CancellationToken);
                return new GetOrderStateResponse { State = Converters.ConvertOrderState(state) };
            }
            catch (NotFoundException ex)
            {
                throw new RpcException(new Status(StatusCode.NotFound, ex.Message));
            }
        }

        public override async Task<GetRegionsListResponse> GetRegionsList(GetRegionsListRequest request, ServerCallContext context)
        {
            var regions = await _regionsRepository.GetRegionsListAsync(context.CancellationToken);
            GetRegionsListResponse result = new();
            result.Regions.Add(regions);
            return result;
        }

        public override async Task<GetOrdersListResponse> GetOrdersList(GetOrdersListRequest request, ServerCallContext context)
        {
            var regions = await _regionsRepository.FindNotPresentedAsync(request.Regions.ToList());
            if (regions.Any())
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, $"Followed region(s) was not found: {string.Join(',', regions)}"));
            }
          //  var filteredByRegions = _regionsRepository.
            var result = await _ordersRepository.GetOrdersListAsync(request.Regions.ToList(), Converters.ConvertOrderType(request.OrderType),
                Converters.ConvertPaginationParameters(request.PaginationParameters),
                Converters.ConvertSortOrder(request.SortingOrder), request.SortingField.ToList(), context.CancellationToken);
            var items = new GetOrdersListResponse();
            items.OrderItem.Add(result.Select(Converters.ConvertOrderEntity));
            return items;
        }

        public override async Task<GetOrdersByRegionsResponse> GetOrdersByRegions(GetOrdersByRegionsRequest request, ServerCallContext context)
        {
            var regions = await _regionsRepository.FindNotPresentedAsync(request.Regions.ToList());
            if (regions.Any())
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, $"Followed region(s) was not found: {string.Join(',', regions)}"));
            }
            var orders = await _ordersRepository.GetOrdersByRegionsAsync(request.StartDate.ToDateTime(), request.Regions.ToList(), context.CancellationToken);
            var result = new GetOrdersByRegionsResponse();
            result.OrderItems.Add(orders.Select(Converters.ConvertRegionOrderItem));
            return result;
        }

        public override async Task<GetOrdersByClientIdResponse> GetOrdersByClientId(GetOrdersByClientIdRequest request, ServerCallContext context)
        {
            var orders = await _ordersRepository.GetOrdersByClientIdAsync(request.ClientId, request.StartDate.ToDateTime(),
                Converters.ConvertPaginationParameters(request.PaginationParameters));
            var result = new GetOrdersByClientIdResponse();
            result.OrderItems.Add(orders.Select(Converters.ConvertOrderEntity));
            return result;
        }
    }
}
