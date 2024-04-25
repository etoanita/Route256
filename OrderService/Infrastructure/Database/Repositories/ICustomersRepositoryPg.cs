using Ozon.Route256.Practice.OrderService.Dal.Models;

namespace Ozon.Route256.Practice.OrderService.Infrastructure.Database.Repositories;

internal interface ICustomersRepositoryPg
{
    Task<CustomerDal?> Find(int customerId, CancellationToken token = default);
    Task<CustomerDal[]> FindMany(List<int> ids, CancellationToken token = default);
    Task CreateOrUpdate(int customerId,
        string customerName,
        string customerSurname,
        string address,
        string phone, CancellationToken token);
}