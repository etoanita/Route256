namespace Ozon.Route256.Practice.OrdersService.DataAccess
{
    public interface ICustomersRepository
    {
        Task<Customer?> Find(long customerId, CancellationToken token);
        Task Insert(Customer customerId, CancellationToken token);

    }
}
