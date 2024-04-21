using Ozon.Route256.Practice.OrderService.Domain;

namespace Ozon.Route256.Practice.OrderService.Domain;

public sealed class OrderAggregate
{
    public Order Order { get; }
    public Customer Customer { get; }

    private OrderAggregate(
    Order order,
    Customer customer
)
    {
        Order = order;
        Customer = customer;
    }

    public static OrderAggregate CreateInstance(Order order, Customer customer)
    {
        if (order.CustomerId != customer.Id)
            throw new ArgumentException();
        return new OrderAggregate(order, customer);
    }
}
