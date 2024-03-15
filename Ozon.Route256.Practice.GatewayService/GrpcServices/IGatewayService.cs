namespace Ozon.Route256.Practice.GatewayService.GrpcServices
{
    public interface IGatewayService
    {
        void CancelOrder(long orderId);
        string GetOrderStatus(long orderId);
        void GetOrders();
        void GetOrdersByRegion(string region);
        void GetOrdersByUser(int userId);
        void GetClients();
        List<string> GetRegions();
    }
}
