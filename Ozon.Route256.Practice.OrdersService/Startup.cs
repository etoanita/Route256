using Npgsql;
using OpenTelemetry.Exporter;
using OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Ozon.Route256.Practice.LogisticsSimulator.Grpc;
using Ozon.Route256.Practice.OrderService.Application;
using Ozon.Route256.Practice.OrderService.Infrastructure;
using Serilog;
using System.Reflection;
using Google.Protobuf.WellKnownTypes;
using Ozon.Route256.Practice.CustomerService.Infrastructure.Tracing;

namespace Ozon.Route256.Practice.OrdersService
{
    public sealed class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .Enrich.WithMemoryUsage()
            .CreateLogger();
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddGrpc(option =>
            {
                option.Interceptors.Add<LoggerInterceptor>();
                option.Interceptors.Add<TracingInterceptor>();
            });
            serviceCollection.AddGrpcClient<SdService.SdServiceClient>(option =>
            {
                var url = _configuration.GetValue<string>("ROUTE256_SD_ADDRESS");
                if (string.IsNullOrEmpty(url))
                {
                    throw new ArgumentException("ROUTE256_SD_ADDRESS variable is null or empty");
                }

                option.Address = new Uri(url);
            });
            serviceCollection.AddGrpcClient<LogisticsSimulatorService.LogisticsSimulatorServiceClient>(option =>
            {
                var url = _configuration.GetValue<string>("LOGISTICS_SIMULATOR_ADDRESS");
                if (string.IsNullOrEmpty(url))
                {
                    throw new ArgumentException("LOGISTICS_SIMULATOR_ADDRESS variable is null or empty");
                }

                option.Address = new Uri(url);
            });
            serviceCollection.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
            serviceCollection.AddGrpcReflection();
            serviceCollection.AddControllers();
            serviceCollection.AddEndpointsApiExplorer();
            serviceCollection.AddScoped<IOrderService, OrderServiceAdapter>();
            serviceCollection.AddScoped<IRegionService, RegionServiceAdapter>();
            serviceCollection.AddSingleton<IContractsMapper, ContractsMapper>();
            serviceCollection.AddApplication();
            serviceCollection.AddInfrastructure(_configuration);
            serviceCollection.AddOpenTelemetry()
                .WithTracing(builder => builder
                .AddAspNetCoreInstrumentation()
                .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(nameof(OrdersService)))
                .AddNpgsql()
                .AddConsoleExporter()
                .AddSource("Create Order Mapper")
                .AddSource("Create Order Command")
                .AddSource("Grpc Interceptor")
                .AddJaegerExporter(options =>
                {
                    options.AgentHost = "localhost";
                    options.AgentPort = 6831;
                    options.Protocol = JaegerExportProtocol.UdpCompactThrift;
                    options.ExportProcessorType = ExportProcessorType.Simple;
                }));
        }

        public void Configure(IApplicationBuilder applicationBuilder)
        {
            applicationBuilder.UseRouting();
         
            applicationBuilder.UseEndpoints(endpointRouteBuilder =>
            {
                endpointRouteBuilder.MapGrpcReflectionService();
                endpointRouteBuilder.MapGrpcService<OrderService.Infrastructure.GrpcServices.OrdersService>();
            });
        }
    }
}
