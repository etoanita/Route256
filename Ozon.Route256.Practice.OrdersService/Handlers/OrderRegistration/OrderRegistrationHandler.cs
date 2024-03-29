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

    internal OrderRegistrationHandler(IOrdersRepository orderRepository, IRegionsRepository regionsRepository, CustomersClient customersClient, IOrderProducer producer)
    {
        _orderRepository = orderRepository;
        _regionsRepository = regionsRepository;
        _customersClient = customersClient;
        _producer = producer; 
    }

    public async Task Handle(Order order, CancellationToken token)
    {
        var orderAlreadyRegistered = await _orderRepository.IsExistsAsync(order.Id, token);

        if (orderAlreadyRegistered)
            throw new Exception($"Order {order.Id} already registered");

        var customer = await _customersClient.GetCustomerByIdAsync(new GetCustomerByIdRequest { Id = order.Customer.Id }, cancellationToken: token);
        var orderEntity = Converters.CreateOrderEntity(order, DateTime.UtcNow);
        await _orderRepository.InsertAsync(orderEntity, token);


        if (IsOrderValid(order))
        {
            await _producer.ProduceAsync( new[] { order }, token);
        }
    }

    private bool IsOrderValid(Order order)
    {
        return true;
        //return Math.Acos(Math.Sin(lat1) * sin(lat2) + cos(lat1) * cos(lat2) * cos(lon2 - lon1)) * 6371 < 5000;
    }
}