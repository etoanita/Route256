using MediatR;

namespace Ozon.Route256.Practice.OrderService.Application.Commands
{
    public class CancelOrderCommand : IRequest<Unit>
    {
        public CancelOrderCommand(long orderId) => OrderId = orderId;

        public long OrderId { get; }
    }
}
