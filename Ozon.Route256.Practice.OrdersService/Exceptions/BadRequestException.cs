namespace Ozon.Route256.Practice.OrdersService.Exceptions
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
