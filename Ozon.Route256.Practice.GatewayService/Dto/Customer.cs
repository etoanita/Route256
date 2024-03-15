namespace Ozon.Route256.Practice.GatewayService.Dto
{
    public class CustomerDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public AddressDto DefaultAddress { get; set; }
        public List<AddressDto> Addressed { get; set; }
    }

    public class AddressDto
    {
        public string Region { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string Building { get; set; }
        public string Apartment { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
