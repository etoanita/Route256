using Ozon.Route256.Practice.OrdersService.DataAccess;
using Ozon.Route256.Practice.OrdersService.Infrastructure.GrpcServices;
using Ozon.Route256.Practice.OrdersService.Infrastructure.Kafka.Models;
using Ozon.Route256.Practice.OrdersService.Infrastructure.Kafka.Producers;
using static Ozon.Route256.Practice.Customers;

namespace Ozon.Route256.Practice.OrdersService.Handlers.OrderRegistration;

public class OrderRegistrationHandler : IOrderRegistrationHandler
{
    private readonly IOrdersRepository _orderRepository;
    private readonly IRegionsRepository _regionsRepository;
    private readonly CustomersClient _customersClient;
    private readonly IOrderProducer _producer;

    public OrderRegistrationHandler(IOrdersRepository orderRepository, IRegionsRepository regionsRepository, CustomersClient customersClient, IOrderProducer producer)
    {
        _orderRepository = orderRepository;
        _regionsRepository = regionsRepository;
        _customersClient = customersClient;
        _producer = producer; 
    }

    public async Task Handle(NewOrder order, CancellationToken token)
    {
        var orderAlreadyRegistered = await _orderRepository.IsExistsAsync(order.Id, token);

        if (orderAlreadyRegistered)
            throw new Exception($"Order {order.Id} already registered");
        var result  = await _customersClient.GetCustomerByIdAsync(new GetCustomerByIdRequest { Id = order.Customer.Id }, cancellationToken: token);
        var orderEntity = Converters.CreateOrderEntity(order, result.Customer, DateTime.UtcNow);
        await _orderRepository.InsertAsync(orderEntity, token);

        var custAddress = order.Customer.Address;
        var region = await _regionsRepository.FindRegionAsync(order.Customer.Address.Region);
        var depot = region.Depots.First();
        if (IsOrderValid(custAddress.Latitude, custAddress.Longitude, depot.orderLatitude, depot.orderLongitude))
        {
            await _producer.ProduceAsync( new[] { new OrderShort(order.Id) }, token);
        }
    }

    private static bool IsOrderValid(double orderLatitude, double orderLongitude, double depotLatitude, double depotLongitude)
    {
        return Math.Acos(Math.Sin(orderLatitude) * Math.Sin(depotLatitude) + Math.Cos(orderLatitude) * Math.Cos(depotLatitude) * Math.Cos(depotLongitude - orderLongitude)) * 6371 < 5000;
    }
}