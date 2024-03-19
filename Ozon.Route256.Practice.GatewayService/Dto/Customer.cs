namespace Ozon.Route256.Practice.GatewayService
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
}
