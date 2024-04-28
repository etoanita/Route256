using Ozon.Route256.Practice.OrderService.Application;
using Ozon.Route256.Practice.OrderService.Domain;
using Ozon.Route256.Practice.OrderService.Infrastructure.Database.Mappers;
using Ozon.Route256.Practice.OrderService.DataAccess.Postgres;
using Ozon.Route256.Practice.OrderService.Exceptions;
using OrderState = Ozon.Route256.Practice.OrderService.Dal.Models.OrderState;
using Ozon.Route256.Practice.OrderService.Infrastructure.Database.Repositories;
using Ozon.Route256.Practice.OrderService.Application.Metrics;

namespace Ozon.Route256.Practice.OrderService.DataAccess
{
    internal sealed class UnitOfWork : IUnitOfWork
    {
        private readonly IOrdersRepositoryPg _ordersDbAccess;
        private readonly ICustomersRepositoryPg _customerDbAccess;
        private readonly IDataWriteMapper _mapper;
        private readonly IOrderMetrics _metrics;

        public UnitOfWork(
            IOrdersRepositoryPg ordersDbAccess,
            ICustomersRepositoryPg customerDbAccess,
            IDataWriteMapper mapper, IOrderMetrics metrics)
        {
            _ordersDbAccess = ordersDbAccess;
            _customerDbAccess = customerDbAccess;
            _mapper = mapper;
            _metrics = metrics;
        }
        public async Task CancelOrder(long orderId, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();

            var order = await _ordersDbAccess.Find(orderId) ?? throw new NotFoundException($"Order with id={orderId} not found");

            if (order.State != Dal.Models.OrderState.SentToCustomer && order.State != OrderState.Created)
                throw new BadRequestException($"Cannot cancel order {orderId}. " +
                    $"Order is in inappropriate state.");

            order.State = Dal.Models.OrderState.Cancelled;
            await _ordersDbAccess.UpdateOrderState(order.Id, OrderState.Cancelled, ct);
        }

        public async Task SaveOrder(OrderAggregate order, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();
           
            await _customerDbAccess.CreateOrUpdate(order.Customer.Id, order.Customer.FirstName, order.Customer.LastName, order.Customer.Address, order.Customer.Phone, ct);
            await _ordersDbAccess.Insert(_mapper.ToOrderDao(order.Order), ct);
            _metrics.OrderCreated(order.Order.OrderType);
        }

        public async Task UpdateOrderState(long orderId, Domain.OrderState state, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();
            var order = await _ordersDbAccess.Find(orderId, ct);
            bool isExists = order != null;
            if (!isExists )
            {
                throw new Exception($"Order {orderId} was not found");
            }
            await _ordersDbAccess.UpdateOrderState(orderId, _mapper.ToOrderStateDal(state), ct);
        }
    }
}
