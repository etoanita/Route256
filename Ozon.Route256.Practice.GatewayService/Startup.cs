using Grpc.Core;
using Grpc.Net.Client;
using Grpc.Net.Client.Balancer;
using Grpc.Net.Client.Configuration;
using Ozon.Route256.Practice.GatewayService.GrpcServices;

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
            var factory = new StaticResolverFactory(addr => new[]
            {
                new BalancerAddress("orders-service-1", 5010),
                new BalancerAddress("orders-service-2", 5010)
            }); 
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
            });
            serviceCollection.AddGrpcReflection();
            serviceCollection.AddControllers();
            serviceCollection.AddEndpointsApiExplorer();
            serviceCollection.AddSwaggerGen();
            serviceCollection.AddScoped<IGatewayService, GatewayService.GrpcServices.GatewayService>();
            
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
