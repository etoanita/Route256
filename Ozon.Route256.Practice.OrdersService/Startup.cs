using Ozon.Route256.Practice.OrdersService.Infrastructure;

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
            serviceCollection.AddGrpc(option => option.Interceptors.Add<LoggerInterceptor>());
            serviceCollection.AddGrpcClient<SdService.SdServiceClient>(option =>
            {
                var url = _configuration.GetValue<string>("ROUTE256_SD_ADDRESS");
                if (string.IsNullOrEmpty(url))
                {
                    throw new ArgumentException("ROUTE256_SD_ADDRESS variable is null or empty");
                }

                option.Address = new Uri(url);
            });
            
            serviceCollection.AddGrpcReflection();
            serviceCollection.AddControllers();
            serviceCollection.AddEndpointsApiExplorer();
        }

        public void Configure(IApplicationBuilder applicationBuilder)
        {
            applicationBuilder.UseRouting();
            applicationBuilder.UseHttpsRedirection();
            applicationBuilder.UseAuthorization();
            applicationBuilder.UseEndpoints(endpointRouteBuilder =>
            {
                endpointRouteBuilder.MapGrpcReflectionService();
                endpointRouteBuilder.MapGrpcService<GrpcServices.OrdersService>();
            });
        }
    }
}
