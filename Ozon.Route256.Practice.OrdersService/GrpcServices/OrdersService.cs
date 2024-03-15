using Grpc.Core;
using Ozon.Route256.Practice.OrdersService.Exceptions;

namespace Ozon.Route256.Practice.OrdersService.GrpcServices
{
    public sealed class OrdersService : Orders.OrdersBase
    {
        public override Task<CancelOrderResponse> CancelOrder(CancelOrderRequest request, ServerCallContext context)
        {
            throw new NotFoundException($"Order with id = {request.OrderId} was not found");
        }

        public override Task<GetOrderStateResponse> GetOrderState(GetOrderStateRequest request, ServerCallContext context)
        {
            throw new NotFoundException($"Order with id = {request.OrderId} was not found");
        }

        public override Task<GetRegionsListResponse> GetRegionsList(GetRegionsListRequest request, ServerCallContext context)
        {
            return Task.FromResult(new GetRegionsListResponse());
        }

        public override Task<GetOrdersListResponse> GetOrdersList(GetOrdersListRequest request, ServerCallContext context)
        {
            return Task.FromResult(new GetOrdersListResponse());
        }

        public override Task<GetOrdersByRegionsResponse> GetOrdersByRegions(GetOrdersByRegionsRequest request, ServerCallContext context)
        {
            return Task.FromResult(new GetOrdersByRegionsResponse());
        }

        public override Task<GetOrdersByClientIdResponse> GetOrdersByClientId(GetOrdersByClientIdRequest request, ServerCallContext context)
        {
            return Task.FromResult(new GetOrdersByClientIdResponse());
        }
    }
}
