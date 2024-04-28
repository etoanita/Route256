namespace Ozon.Route256.Practice.OrderService.Infrastructure.Metrics;

internal interface IGrpcMetrics
{
    void WriteResponseTime(string method, long elapsedMs, bool isSuccess);
}
