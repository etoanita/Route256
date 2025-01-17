﻿using FluentValidation;
using Grpc.Core;
using Grpc.Net.Client;
using Grpc.Net.Client.Balancer;
using Grpc.Net.Client.Configuration;
using Grpc.Net.ClientFactory;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Ozon.Route256.Practice.GatewayService;
using Ozon.Route256.Practice.GatewayService.GrpcServices;
using Ozon.Route256.Practice.GatewayService.Infrastructure;
using System.Reflection;

namespace Ozon.Route256.Practice.OrdersService
{
    public sealed class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection serviceCollection)
        {
            //TODO: handle parameters correctly
            var os1 = _configuration.GetValue<string>("ORDERS_SERVICE_1");
            if (string.IsNullOrEmpty(os1))
            {
                throw new ArgumentException("ORDERS_SERVICE_1 variable is null or empty");
            }
            var os2 = _configuration.GetValue<string>("ORDERS_SERVICE_2");
            if (string.IsNullOrEmpty(os1))
            {
                throw new ArgumentException("ORDERS_SERVICE_2 variable is null or empty");
            }
            var os1Splitted = os1.Split(':');
            var os2Splitted = os2.Split(':');
            var factory = new StaticResolverFactory(addr => new[]
            {
                new BalancerAddress(os1Splitted[0], int.Parse(os1Splitted[1])),
                new BalancerAddress(os2Splitted[0], int.Parse(os2Splitted[1]))
            }); ; 
            serviceCollection.AddGrpcClient<Orders.OrdersClient>(options =>
            {
                options.Address = new Uri(_configuration.GetValue<string>("ROUTE256_ORDERS_SERVICE_GRPC"));
            }).ConfigureChannel(x =>
            {
                x.Credentials = ChannelCredentials.Insecure;
                x.ServiceConfig = new ServiceConfig
                {
                    LoadBalancingConfigs = { new LoadBalancingConfig("round_robin") }
                };
            }).AddInterceptor<LoggerInterceptor>(InterceptorScope.Client);
            serviceCollection.AddGrpcClient <Customers.CustomersClient> (options =>
            {
                options.Address = new Uri(_configuration.GetValue<string>("ROUTE256_CUSTOMER_SERVICE_GRPC"));
            });
            serviceCollection.AddGrpcReflection();

            serviceCollection.AddControllers(options =>
            {
                options.Filters.Add<HttpResponseExceptionFilter>();
            }).ConfigureApiBehaviorOptions(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var problems = new CustomBadRequestModel(context);
                    return new BadRequestObjectResult(problems);
                };
            }); ;
            serviceCollection.AddEndpointsApiExplorer();
            serviceCollection.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "ROUTE.256",
                    Description = "Project for Route.256",
                });
                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            });

            serviceCollection.AddScoped<IOrderService, OrderSevice>();
            serviceCollection.AddScoped<ICustomerService, CustomerService>();
            serviceCollection.AddScoped<IValidator<GetOrdersRequestParametersDto>, OrderRequestValidator>();
            serviceCollection.AddSingleton<LoggerInterceptor>();
            serviceCollection.AddSingleton<ResolverFactory>(factory);
        }

        public void Configure(IApplicationBuilder applicationBuilder)
        {
            applicationBuilder.UseRouting();
            applicationBuilder.UseSwagger();
            applicationBuilder.UseSwaggerUI();
            applicationBuilder.UseHttpsRedirection();
            applicationBuilder.UseAuthorization();

            applicationBuilder.UseEndpoints(endpointRouteBuilder =>
            {
                endpointRouteBuilder.MapControllers();
            });
        }
    }
}
