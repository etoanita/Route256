﻿using System.Diagnostics;
using Grpc.Core;
using Grpc.Core.Interceptors;

namespace Ozon.Route256.Practice.CustomerService.Infrastructure.Tracing;

public class TracingInterceptor : Interceptor
{
    public override Task<TResponse> UnaryServerHandler<TRequest, TResponse>(TRequest request, ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation)
    {
        using var grpcActivity = new ActivitySource("Grpc Interceptor").StartActivity(
            name: context.Method,
            kind: ActivityKind.Internal,
            tags: new List<KeyValuePair<string, object?>>
            {
                new ("grpc_request", request),
                new ("grpc_headers", context.RequestHeaders)
            });

        return base.UnaryServerHandler(request, context, continuation);
    }
}
