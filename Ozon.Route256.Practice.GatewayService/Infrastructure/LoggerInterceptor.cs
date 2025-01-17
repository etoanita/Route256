using Grpc.Core;
using Grpc.Core.Interceptors;

namespace Ozon.Route256.Practice.GatewayService.Infrastructure;

public sealed class LoggerInterceptor : Interceptor
{
    private readonly ILogger<LoggerInterceptor> _logger;

    public LoggerInterceptor(ILogger<LoggerInterceptor> logger)
    {
        _logger = logger;
    }

    public override AsyncUnaryCall<TResponse> AsyncUnaryCall<TRequest, TResponse>(TRequest request, ClientInterceptorContext<TRequest, TResponse> context, AsyncUnaryCallContinuation<TRequest, TResponse> continuation)
    {
        _logger.LogInformation("Request {request}", request);

        try
        {
            var response = base.AsyncUnaryCall(request, context, continuation);
            
            _logger.LogInformation("Response {response}", response);

            return response;
        }
        catch (RpcException ex)
        {
            _logger.LogError(ex.Message);
            throw;
        }
    }
}