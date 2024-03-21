namespace Ozon.Route256.Practice.GatewayService
{
    public record CustomerDto 
    (
        int Id,
        string FirstName,
        string LastName,
        string MobileNumber,
        string Email,
        AddressDto DefaultAddress,
        List<AddressDto> Addressed
    );
}
