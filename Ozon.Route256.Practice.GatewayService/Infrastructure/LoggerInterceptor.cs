using Grpc.Core;
using Grpc.Core.Interceptors;
using Ozon.Route256.Practice.GatewayService.Exceptions;
using System.Net;
using System.Net.Mail;
using System.Web.Http;

namespace Ozon.Route256.Practice.GatewayService.Infrastructure;

public sealed class LoggerInterceptor : Interceptor
{
    private readonly ILogger<LoggerInterceptor> _logger;

    public LoggerInterceptor(ILogger<LoggerInterceptor> logger)
    {
        _logger = logger;
    }

    public override TResponse BlockingUnaryCall<TRequest, TResponse>(TRequest request, ClientInterceptorContext<TRequest, TResponse> context, BlockingUnaryCallContinuation<TRequest, TResponse> continuation)
    {
        _logger.LogInformation("Request {request}", request);

        try
        {
            var response = base.BlockingUnaryCall(request, context, continuation);
            
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