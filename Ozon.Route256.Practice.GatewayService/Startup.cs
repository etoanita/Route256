using Grpc.Core;
using Grpc.Net.Client;
using Grpc.Net.Client.Balancer;
using Grpc.Net.Client.Configuration;
using Grpc.Net.ClientFactory;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
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
            var os1 = _configuration.GetValue<string>("ORDERS_SERVICE_1").Split(':');
            var os2 = _configuration.GetValue<string>("ORDERS_SERVICE_2").Split(':');
            var factory = new StaticResolverFactory(addr => new[]
            {
                new BalancerAddress(os1[0], int.Parse(os1[1])),
                new BalancerAddress(os2[0], int.Parse(os2[1]))
            }); ; 
            serviceCollection.AddSingleton<ResolverFactory>(factory);
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
            });
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
            serviceCollection.AddSingleton<LoggerInterceptor>();
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
