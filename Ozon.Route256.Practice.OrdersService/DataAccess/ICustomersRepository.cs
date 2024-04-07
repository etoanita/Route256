namespace Ozon.Route256.Practice.OrdersService.DataAccess
{
    internal interface ICustomersRepository
    {
        Task<Customer?> Find(long customerId, CancellationToken token);
        Task Insert(Customer customer, CancellationToken token);

    }
}
