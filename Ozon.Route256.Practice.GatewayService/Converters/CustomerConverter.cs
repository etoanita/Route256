using Ozon.Route256.Practice.GatewayService.Dto;

namespace Ozon.Route256.Practice.GatewayService.Converters
{
    public static class CustomerConverter
    {
        public static CustomerDto Convert(this Customer customer)
        {
            CustomerDto result = new CustomerDto();
            result.Id = customer.Id;
            result.FirstName = customer.FirstName;
            result.LastName = customer.LastName;
            result.MobileNumber = customer.MobileNumber;
            result.Email = customer.Email;
            result.DefaultAddress = customer.DefaultAddress.Convert();
            result.Addressed = customer.Addressed.Select(Convert).ToList();
            return result;
        }

        public static AddressDto Convert(this Address address)
        {
            AddressDto result = new AddressDto();
            result.Region = address.Region;
            result.City = address.City;
            result.Street = address.Street;
            result.Building = address.Building;
            result.Apartment = address.Apartment;
            result.Latitude = address.Latitude;
            result.Longitude = address.Longitude;
            return result;
        }
    }
}
