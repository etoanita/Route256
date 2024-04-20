using MediatR;
using Ozon.Route256.Practice.OrderService.Application.Mappers;

namespace Ozon.Route256.Practice.OrderService.Application.Commands
{
    internal sealed class CancelOrderCommandHandler : IRequestHandler<CancelOrderCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICommandMapper _commandMapper;

        public CancelOrderCommandHandler(IUnitOfWork unitOfWork, ICommandMapper commandMapper)
        {
            _unitOfWork = unitOfWork;
            _commandMapper = commandMapper;
        }

        public async Task<Unit> Handle(CancelOrderCommand request, CancellationToken cancellationToken)
        {
            await _unitOfWork.CancelOrder(request.OrderId, cancellationToken);

            return Unit.Value;
        }
    }

}
