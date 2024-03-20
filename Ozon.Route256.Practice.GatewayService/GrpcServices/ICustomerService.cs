namespace Ozon.Route256.Practice.GatewayService.GrpcServices
{
    public interface ICustomerService
    {
        Task<List<CustomerDto>> GetClients();
    }
}
