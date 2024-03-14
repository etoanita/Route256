using Grpc.Core;

namespace Ozon.Route256.Practice.OrdersService.GrpcServices
{
    public sealed class OrdersService : Orders.OrdersBase
    {
        public override Task<CancelOrderResponse> CancelOrder(CancelOrderRequest request, ServerCallContext context)
        {
            return base.CancelOrder(request, context);
        }
    }
}
