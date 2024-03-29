namespace Ozon.Route256.Practice.OrdersService.Infrastructure.Kafka.Models
{
    public record struct Order(
        long Id,
        int Source,
        Customer Customer,
        List<Good> Goods
    );

    public record struct Customer(
        int Id,
        Address Address
    );

    public record struct Address(
        string Region,
        string City,
        string Street,
        int Building,
        int Apartment,
        double Latitude,
        double Longitude
    );

    public record struct Good(
        long Id,
        string Name,
        int Quantity,
        int Price, 
        int Weight
    );
}
