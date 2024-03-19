using Ozon.Route256.Practice.GatewayService.Converters;
using static Ozon.Route256.Practice.Customers;

namespace Ozon.Route256.Practice.GatewayService.GrpcServices
{
    public class CustomerService : ICustomerService
    {
        private readonly Customers.CustomersClient _customersClient;
        public CustomerService(Customers.CustomersClient customersClient)
        {
            _customersClient = customersClient;
        }
        public async Task<List<CustomerDto>> GetClients()
        {
            var result = await _customersClient.GetCustomersAsync(new GetCustomersRequest());
            return result.Customers.Select(CustomerConverter.Convert).ToList();
        }
    }
}
