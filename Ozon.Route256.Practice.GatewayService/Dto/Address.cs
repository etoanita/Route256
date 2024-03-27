using System.ComponentModel.DataAnnotations;

namespace Ozon.Route256.Practice.GatewayService
{
    public record AddressDto
    (
        string Region,
        string City,
        string Street,
        string Building,
        string Apartment,
        double Latitude,
        double Longitude
    );
}
