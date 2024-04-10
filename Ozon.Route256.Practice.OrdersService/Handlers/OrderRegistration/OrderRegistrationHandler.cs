using Ozon.Route256.Practice.OrdersService.DataAccess;
using Ozon.Route256.Practice.OrdersService.Infrastructure.GrpcServices;
using Ozon.Route256.Practice.OrdersService.Infrastructure.Kafka.Models;
using Ozon.Route256.Practice.OrdersService.Infrastructure.Kafka.Producers;
using static Ozon.Route256.Practice.Customers;

namespace Ozon.Route256.Practice.OrdersService.Handlers.OrderRegistration;

internal class OrderRegistrationHandler : IOrderRegistrationHandler
{
    private readonly IOrdersRepository _orderRepository;
    private readonly IRegionsRepository _regionsRepository;
    private readonly ICustomersRepository _customersRepository;
    private readonly CustomersClient _customersClient;
    private readonly IOrderProducer _producer;
    private readonly ILogger _logger;

    public OrderRegistrationHandler(IOrdersRepository orderRepository, IRegionsRepository regionsRepository, 
        CustomersClient customersClient, IOrderProducer producer, ICustomersRepository customersRepository,
        ILogger<OrderRegistrationHandler> logger)
    {
        _orderRepository = orderRepository;
        _regionsRepository = regionsRepository;
        _customersRepository = customersRepository;
        _customersClient = customersClient;
        _producer = producer; 
        _logger = logger;
    }

    public async Task Handle(NewOrder order, CancellationToken token)
    {
        var orderAlreadyRegistered = await _orderRepository.IsExistsAsync(order.Id, token);

        if (orderAlreadyRegistered)
            throw new Exception($"Order {order.Id} already registered");

        Customer? customer;
        try
        {
            customer = await _customersRepository.Find(order.Customer.Id, token);
            if (customer == null)
            {
                var result = await _customersClient.GetCustomerByIdAsync(new GetCustomerByIdRequest { Id = order.Customer.Id }, cancellationToken: token);
                customer = result.Customer;
                await _customersRepository.Insert(customer, token);
            }
            else
                _logger.LogInformation("Get customer data from redis cache");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message); //TODO: handle correctly
            customer = new Customer
            {
                Id = order.Customer.Id,
                FirstName = "TestCustFirstName",
                LastName = "TestCustLastName",
                MobileNumber = "12344567890"
            };
        }
        var orderEntity = Converters.CreateOrderEntity(order, customer, DateTime.UtcNow);
        await _orderRepository.InsertAsync(orderEntity, token);

        var custAddress = order.Customer.Address;
        var region = await _regionsRepository.FindRegionAsync(order.Customer.Address.Region, token);
        var depot = region.Depots.First();
        if (IsOrderValid(custAddress.Latitude, custAddress.Longitude, depot.OrderLatitude, depot.OrderLongitude))
        {
            await _producer.ProduceAsync( new[] { new OrderShort(order.Id) }, token);
        }
        else
        {
            _logger.LogWarning("Order {OrderId} is not valid", order.Id);
        }
    }

    private static bool IsOrderValid(double orderLatitude, double orderLongitude, double depotLatitude, double depotLongitude)
    {
        return Math.Acos(Math.Sin(orderLatitude) * Math.Sin(depotLatitude) + Math.Cos(orderLatitude) * Math.Cos(depotLatitude) * Math.Cos(depotLongitude - orderLongitude)) * 6371 < 5000;
    }
}