namespace Ozon.Route256.Practice.GatewayService.GrpcServices
{
    public class GatewayService : IGatewayService
    {
        private readonly Orders.OrdersClient _ordersClient;
        private readonly Customers.CustomersClient _customersClient;
        public GatewayService(Orders.OrdersClient ordersClient, Customers.CustomersClient customersClient)
        {
            _ordersClient = ordersClient;
            _customersClient = customersClient;
        }

        public void CancelOrder(long orderId)
        {
            _ordersClient.CancelOrder(new CancelOrderRequest { OrderId = orderId });
        }

        public string GetOrderStatus(long orderId)
        {
            var request = _ordersClient.GetOrderState(new GetOrderStateRequest { OrderId = orderId });
            return request.State;
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
           // return _customersClient.GetCustomers().Customers.ToList()
        }

        public List<string> GetRegions()
        {
            return _ordersClient.GetRegionsList(new GetRegionsListRequest()).Regions.ToList();
        }

    }
}
