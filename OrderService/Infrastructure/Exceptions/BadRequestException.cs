namespace Ozon.Route256.Practice.OrderService.Exceptions
{
    public class BadRequestException : Exception
    {
        public override string Message { get; }

        public BadRequestException(string exceptionMessage)
        {
            Message = exceptionMessage;
        }
    }
}
