using MediatR;
using Ozon.Route256.Practice.OrderService.Application.Commands;
using Ozon.Route256.Practice.OrderService.Application.Queries;

namespace Ozon.Route256.Practice.OrderService.Application;

internal sealed class OrderServiceAdapter : IOrderService
{
    private readonly IMediator _mediator;
    private readonly IContractsMapper _mapper;

    public OrderServiceAdapter(IMediator mediator, IContractsMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    public async Task CancelOrder(long orderId, CancellationToken cancellationToken)
    {
        await _mediator.Send(new CancelOrderCommand(orderId), cancellationToken);
    }

    public async Task CreateOrder(CreateOrderRequest request, CancellationToken cancellationToken)
    {
        var command = _mapper.ToCommand(request);
        await _mediator.Send(new CreateOrderCommand(command), cancellationToken);
    }

    public async Task<OrderState> GetOrderState(long orderId, CancellationToken cancellationToken)
    {
        var state = await _mediator.Send(new GetOrderStateQuery(), cancellationToken);
        return _mapper.ToContracts(state);
    }

    public async Task<List<OrderItem>> GetOrders(GetOrdersListRequest request, CancellationToken cancellationToken)
    {
        var orders = await _mediator.Send(new GetOrdersQuery(request.Regions.ToList(),
            _mapper.ToCommand(request.OrderType), _mapper.ToCommand(request.PaginationParameters), 
            _mapper.ToCommand(request.SortingOrder), request.SortingField.ToList()), cancellationToken);
        return orders.Select(_mapper.ToContracts).ToList();
    }


    public async Task<List<OrderItem>> GetOrdersByClientId(GetOrdersByClientIdRequest request, CancellationToken cancellationToken)
    {
        var orders = await _mediator.Send(new GetOrderByClientIdQuery(request.ClientId, request.StartDate.ToDateTime(),
            _mapper.ToCommand(request.PaginationParameters)), cancellationToken);
        return orders.Select(_mapper.ToContracts).ToList();
    }

    public async Task<List<RegionOrderItem>> GetOrdersByRegions(GetOrdersByRegionsRequest request, CancellationToken cancellationToken)
    {
        var orders = await _mediator.Send(new GetOrdersByRegionQuery(request.StartDate.ToDateTime(), request.Regions.ToList()), cancellationToken);
        return orders.Select(_mapper.ToContracts).ToList();
    }
}
