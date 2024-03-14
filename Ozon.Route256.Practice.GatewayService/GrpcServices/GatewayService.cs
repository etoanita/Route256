namespace Ozon.Route256.Practice.GatewayService.GrpcServices
{
    public class GatewayService : IGatewayService
    {
        private readonly Orders.OrdersClient _ordersClient;
        public GatewayService(Orders.OrdersClient ordersClient)
        {
            _ordersClient = ordersClient;
        }

        public void CancelOrder(long orderId)
        {
            _ordersClient.CancelOrder(new CancelOrderRequest());
        }

        public string GetOrderStatus(long orderId)
        {
            throw new NotImplementedException();
        }

        public void GetOrders()
        {
            throw new NotImplementedException();
        }

        public void GetOrdersByRegion(string region)
        {
            throw new NotImplementedException();
        }

        public void GetOrdersByUser(int userId)
        {
            throw new NotImplementedException();
        }

        public void GetClients()
        {
            throw new NotImplementedException();
        }

        public void GetRegions()
        {
            throw new NotImplementedException();
        }
    }
}
