using Ozon.Route256.Practice.OrdersService.Dal.Common.Shard;
using Ozon.Route256.Practice.LogisticsSimulator.Grpc;
using Ozon.Route256.Practice.OrdersService.Bll;
using Ozon.Route256.Practice.OrdersService.ClientBalancing;
using Ozon.Route256.Practice.OrdersService.Configurations;
using Ozon.Route256.Practice.OrdersService.Dal.Common;
using Ozon.Route256.Practice.OrdersService.DataAccess;
using Ozon.Route256.Practice.OrdersService.DataAccess.Postgres;
using Ozon.Route256.Practice.OrdersService.Handlers;
using Ozon.Route256.Practice.OrdersService.Infrastructure;
using StackExchange.Redis;

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
            serviceCollection.AddGrpcClient<LogisticsSimulatorService.LogisticsSimulatorServiceClient>(option =>
            {
                var url = _configuration.GetValue<string>("LOGISTICS_SIMULATOR_ADDRESS");
                if (string.IsNullOrEmpty(url))
                {
                    throw new ArgumentException("LOGISTICS_SIMULATOR_ADDRESS variable is null or empty");
                }

                option.Address = new Uri(url);
            });
            serviceCollection.AddGrpcClient<Customers.CustomersClient>(option =>
            {
                var url = _configuration.GetValue<string>("CUSTOMER_SERVICE_ADDRESS");
                if (string.IsNullOrEmpty(url))
                {
                    throw new ArgumentException("CUSTOMER_SERVICE_ADDRESS variable is null or empty");
                }

                option.Address = new Uri(url);
            });
            serviceCollection.AddGrpcReflection();
            serviceCollection.AddControllers();
            serviceCollection.AddEndpointsApiExplorer();
            serviceCollection.AddSingleton<IDbStore, DbStore>();
            serviceCollection.AddHostedService<SdConsumerHostedService>();
            serviceCollection.AddScoped<IRegionsRepository, RegionsRepositoryDatabase>();
            serviceCollection.AddScoped<IOrdersRepository, OrdersRepositoryDatabase>();
            serviceCollection.AddScoped<OrdersDbAccessPg>();
            serviceCollection.AddScoped<RegionsDbAccessPg>();
            serviceCollection.AddScoped<CustomerDbAccessPg>();
            serviceCollection.AddScoped<ICustomersRepository, RedisCustomerRepository>();
            serviceCollection.AddKafka().AddHandlers();
            serviceCollection.Configure<KafkaConfiguration>(_configuration.GetSection(nameof(KafkaConfiguration)));
            serviceCollection.AddSingleton<IConnectionMultiplexer>(
                _ =>
                {
                    var connection = ConnectionMultiplexer.Connect("redis", x => x.AbortOnConnectFail = false);

                    return connection;
                });
            var connectionString = _configuration.GetConnectionString("OrderDatabase");
            //serviceCollection.AddFluentMigratorCore()
            //    .ConfigureRunner(
            //        builder => builder
            //            .AddPostgres()
            //            .ScanIn(GetType().Assembly)
            //            .For.Migrations())
            //    .AddOptions<ProcessorOptions>()
            //    .Configure(
            //        options =>
            //        {
            //            options.ProviderSwitches = "Force Quote=false";
            //            options.Timeout = TimeSpan.FromMinutes(10);
            //            options.ConnectionString = connectionString;
            //        });
            PostgresMapping.MapCompositeTypes();

            serviceCollection.AddSingleton<IPostgresConnectionFactory>(_ => new PostgresConnectionFactory(connectionString));

            serviceCollection.Configure<DbOptions>(_configuration.GetSection(nameof(DbOptions)));
            serviceCollection.AddSingleton<IShardPostgresConnectionFactory, ShardConnectionFactory>();
            serviceCollection.AddSingleton<IShardingRule<int>, IntShardingRule>();
          //  serviceCollection.AddSingleton<IShardingRule<string>, StringShardingRule>();
            serviceCollection.AddSingleton<IShardMigrator, ShardMigrator>();

        }

        public void Configure(IApplicationBuilder applicationBuilder)
        {
            applicationBuilder.UseRouting();
            applicationBuilder.UseHttpsRedirection();
            applicationBuilder.UseAuthorization();
            applicationBuilder.UseEndpoints(endpointRouteBuilder =>
            {
                endpointRouteBuilder.MapGrpcReflectionService();
                endpointRouteBuilder.MapGrpcService<Infrastructure.GrpcServices.OrdersService>();
            });
        }
    }
}
