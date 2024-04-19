using Ozon.Route256.Practice.OrderService.Domain.Core;

namespace Ozon.Route256.Practice.OrderService.Domain;

public sealed class Customer : Entity<int>
{
    private Customer(int id, string firstName, string lastName, string address, string phone) : base(id)
    {
        FirstName = firstName;
        LastName = lastName;
        Address = address;
        Phone = phone;
    }

    public static Customer CreateInstance(int id, string firstName, string lastName, string address, string phone)
    {
        return new Customer(id, firstName, lastName, address, phone);
    }

    public string FirstName { get; }

    public string LastName { get; }

    public string Address { get; }
    public string Phone { get; }
}
