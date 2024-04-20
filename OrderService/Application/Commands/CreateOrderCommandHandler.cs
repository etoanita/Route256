using MediatR;
using Ozon.Route256.Practice.OrderService.Application.Mappers;

namespace Ozon.Route256.Practice.OrderService.Application.Commands;

internal sealed class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICommandMapper _commandMapper;

    public CreateOrderCommandHandler(IUnitOfWork unitOfWork, ICommandMapper commandMapper)
    {
        _unitOfWork = unitOfWork;
        _commandMapper = commandMapper;
    }

    public async Task<Unit> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var order = _commandMapper.MapToDomainOrder(request);

        await _unitOfWork.SaveOrder(order, cancellationToken);
        
        return Unit.Value;
    }
}
