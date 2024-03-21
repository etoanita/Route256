namespace Ozon.Route256.Practice.GatewayService.Converters
{
    public static class CustomerConverter
    {
        public static CustomerDto Convert(this Customer customer)
        {
            return new CustomerDto(customer.Id,
                customer.FirstName,
                customer.LastName,
                customer.MobileNumber,
                customer.Email,
                customer.DefaultAddress.Convert(),
                customer.Addressed.Select(Convert).ToList()
            );
        }

        public static AddressDto Convert(this Address address) => new        
        (
            address.Region,
            address.City,
            address.Street,
            address.Building,
            address.Apartment,
            address.Latitude,
            address.Longitude
        );
    }
}
