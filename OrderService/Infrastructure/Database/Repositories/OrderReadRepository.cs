using Ozon.Route256.Practice.OrderService.Application;
using Ozon.Route256.Practice.OrderService.Application.Queries;
using Ozon.Route256.Practice.OrderService.Domain;
using Ozon.Route256.Practice.OrderService.Infrastructure.Database.Mappers;

namespace Ozon.Route256.Practice.OrderService.Infrastructure.Database.Repositories
{
    internal sealed class OrderReadRepository : IOrderReadRepository
    {
        private readonly ICustomersRepositoryPg _customersRepository;
        private readonly IOrdersRepositoryPg _ordersRepository;
        private readonly IDataReadMapper _readMapper;
        private readonly IDataWriteMapper _writeMapper;

        public OrderReadRepository(IOrdersRepositoryPg ordersRepository, ICustomersRepositoryPg customersRepository,
            IDataReadMapper readMapper, IDataWriteMapper writeMapper)
        {
            _ordersRepository = ordersRepository;
            _customersRepository = customersRepository;
            _readMapper = readMapper;
            _writeMapper = writeMapper;
        }
        public async Task<IReadOnlyCollection<OrderInfo>> GetOrdersByClientId(GetOrderByClientIdQuery query, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();

            var result = await _ordersRepository.FindByCustomerId(query.ClientId, query.StartFrom, query.PaginationParameters, ct);
            var customer = await _customersRepository.Find(query.ClientId);
            return result.Select(x => _readMapper.JoinOrderAndCustomer(x, customer)).ToList().AsReadOnly();
        }

        public async Task<IReadOnlyCollection<OrderByRegion>> GetOrdersByRegions(GetOrdersByRegionQuery query, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();

            var result = await _ordersRepository.FindByRegions(query.Regions, query.StartDate, ct);
            return result.Select(_readMapper.ConvertOrderByRegion).ToList().AsReadOnly();
        }

        public async Task<IReadOnlyCollection<OrderInfo>> GetOrders(GetOrdersQuery query, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();

            var result = await _ordersRepository.Find(query.Regions, _writeMapper.ToOrderTypeDal(query.OrderType),
                query.PaginationParameters, query.SortOrder != null ? _writeMapper.ToSortOrderDal(query.SortOrder.Value): null, query.SortingFields, ct);
            var customerIds = result.Select(x => x.CustomerId).Distinct().ToList();
            var customers = await _customersRepository.FindMany(customerIds);
            var custmoersDict = customers.ToDictionary(x => x.Id);
            return result.Select(x => _readMapper.JoinOrderAndCustomer(x, custmoersDict[x.CustomerId])).ToList().AsReadOnly();
        }

        public async Task<OrderState> GetOrderStateAsync(long orderId, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();
            var result = await _ordersRepository.GetOrderState(orderId, ct);
            return _readMapper.ConvertOrderState(result);
        }

        public async Task<bool> IsExistsAsync(long orderId, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();
            var order = await _ordersRepository.Find(orderId, ct);
            return order != null;
        }
    }
}
