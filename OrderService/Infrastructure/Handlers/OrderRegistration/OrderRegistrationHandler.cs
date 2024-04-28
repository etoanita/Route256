using Ozon.Route256.Practice.OrderService.Application;
using Ozon.Route256.Practice.OrderService.DataAccess;
using Ozon.Route256.Practice.OrderService.Domain;
using Ozon.Route256.Practice.OrderService.Infrastructure.Kafka.Models;
using Ozon.Route256.Practice.OrderService.Infrastructure.Kafka.Producers;
using static Ozon.Route256.Practice.Customers;

namespace Ozon.Route256.Practice.OrderService.Handlers.OrderRegistration;

internal class OrderRegistrationHandler : IOrderRegistrationHandler
{
    private readonly IOrderReadRepository _orderRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRegionsRepository _regionsRepository;
    private readonly ICustomersRepository _customersRepository;
    private readonly CustomersClient _customersClient;
    private readonly IOrderProducer _producer;
    private readonly ILogger _logger;

    public OrderRegistrationHandler(IOrderReadRepository orderRepository, IRegionsRepository regionsRepository, 
        CustomersClient customersClient, IOrderProducer producer, ICustomersRepository customersRepository,
        IUnitOfWork unitOfWork, ILogger<OrderRegistrationHandler> logger)
    {
        _orderRepository = orderRepository;
        _regionsRepository = regionsRepository;
        _customersRepository = customersRepository;
        _customersClient = customersClient;
        _producer = producer;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task Handle(NewOrder order, CancellationToken token)
    {
         var orderAlreadyRegistered = await _orderRepository.IsExistsAsync(order.Id, token);

        if (orderAlreadyRegistered)
            throw new Exception($"Order {order.Id} already registered");

        Domain.Customer? customer;
        try
        {
            customer = await _customersRepository.Find(order.Customer.Id, token);

            if (customer == null)
            {
                var result = await _customersClient.GetCustomerByIdAsync(new GetCustomerByIdRequest { Id = order.Customer.Id }, cancellationToken: token);
                customer = Domain.Customer.CreateInstance(result.Customer.Id, result.Customer.FirstName, result.Customer.LastName, result.Customer.DefaultAddress.City, result.Customer.MobileNumber);
                await _customersRepository.Insert(customer, token);
            }
            else
                _logger.LogInformation("Get customer data from redis cache");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message); //TODO: handle correctly
            customer = Domain.Customer.CreateInstance
            (
                id: order.Customer.Id,
                firstName: "TestCustFirstName",
                lastName: "TestCustLastName",
                address: "",
                phone: "12344567890"
            ); ;
        }
        var entityOrder = Domain.Order.CreateInstance(order.Id,
                order.Goods.Count,
                order.Goods.Sum(x => x.Price),
                order.Goods.Sum(x => x.Weight),
                (OrderType)(order.Source - 1), //use converter
                DateTime.UtcNow,
                order.Customer.Address.Region,
                OrderState.Created,
                order.Customer.Id);
        var orderEntity = OrderAggregate.CreateInstance(entityOrder, customer);
        await _unitOfWork.SaveOrder(orderEntity, token);

        var custAddress = order.Customer.Address;
        try
        {
            var region = await _regionsRepository.FindRegionAsync(order.Customer.Address.Region, token);
            var depot = region.Depots.First();
            if (IsOrderValid(custAddress.Latitude, custAddress.Longitude, depot.Latitude, depot.Longitude))
            {
                await _producer.ProduceAsync( new[] { new OrderShort(order.Id) }, token);
            }
            else
            {
                _logger.LogWarning("Order {OrderId} is not valid", order.Id);
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            throw;
        }
    }

    private static bool IsOrderValid(double orderLatitude, double orderLongitude, double depotLatitude, double depotLongitude)
    {
        return Math.Acos(Math.Sin(orderLatitude) * Math.Sin(depotLatitude) + Math.Cos(orderLatitude) * Math.Cos(depotLatitude) * Math.Cos(depotLongitude - orderLongitude)) * 6371 < 5000;
    }
}