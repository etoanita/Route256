using MediatR;
using Ozon.Route256.Practice.OrderService.Application.Metrics;

namespace Ozon.Route256.Practice.OrderService.Application.Commands
{
    internal sealed class CancelOrderCommandHandler : IRequestHandler<CancelOrderCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOrderMetrics _metrics;

        public CancelOrderCommandHandler(IUnitOfWork unitOfWork, IOrderMetrics metrics)
        {
            _unitOfWork = unitOfWork;
            _metrics = metrics;
        }

        public async Task<Unit> Handle(CancelOrderCommand request, CancellationToken cancellationToken)
        {
            await _unitOfWork.CancelOrder(request.OrderId, cancellationToken);
            _metrics.OrderCanceled();
            return Unit.Value;
        }
    }

}
