using MediatR;
using Ozon.Route256.Practice.OrderService.Application.Metrics;

namespace Ozon.Route256.Practice.OrderService.Application.Commands;

internal sealed class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IOrderMetrics _metrics;

    public CreateOrderCommandHandler(IUnitOfWork unitOfWork, IOrderMetrics metrics)
    {
        _unitOfWork = unitOfWork;
        _metrics = metrics;
    }

    public async Task<Unit> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        await _unitOfWork.SaveOrder(request.Order, cancellationToken);
        _metrics.OrderCreated(request.Order.Order.OrderType);
        return Unit.Value;
    }
}
