using Ozon.Route256.Practice.OrderService.Domain;

namespace Ozon.Route256.Practice.OrderService.DataAccess
{
    public interface ICustomersRepository
    {
        Task<Customer?> Find(long customerId, CancellationToken token);
        Task Insert(Customer customer, CancellationToken token);

    }
}
