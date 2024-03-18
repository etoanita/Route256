using Ozon.Route256.Practice.GatewayService.Dto;

namespace Ozon.Route256.Practice.GatewayService.Converters
{
    public static class CustomerConverter
    {
        public static CustomerDto Convert(this Customer customer)
        {
            return new CustomerDto
            {
                Id = customer.Id,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                MobileNumber = customer.MobileNumber,
                Email = customer.Email,
                DefaultAddress = customer.DefaultAddress.Convert(),
                Addressed = customer.Addressed.Select(Convert).ToList()
            };
        }

        public static AddressDto Convert(this Address address)
        {
            return new AddressDto
            {
                Region = address.Region,
                City = address.City,
                Street = address.Street,
                Building = address.Building,
                Apartment = address.Apartment,
                Latitude = address.Latitude,
                Longitude = address.Longitude
            };
        }
    }
}
