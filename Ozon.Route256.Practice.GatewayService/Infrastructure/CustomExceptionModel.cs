using System.Net;

namespace Ozon.Route256.Practice.GatewayService.Infrastructure
{
    public class CustomExceptionModel
    {
        public string? Message { get; set; }
        public string? Source{ get; set; }
        public string? ExceptionType { get; set; }
        public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.InternalServerError;
    }
}
