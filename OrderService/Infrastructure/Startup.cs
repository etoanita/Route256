using Ozon.Route256.Practice.OrderService.Dal.Common.Shard;
using Ozon.Route256.Practice.OrderService.Bll;
using Ozon.Route256.Practice.OrderService.ClientBalancing;
using Ozon.Route256.Practice.OrderService.Configurations;
using Ozon.Route256.Practice.OrderService.Dal.Common;
using Ozon.Route256.Practice.OrderService.DataAccess;
using Ozon.Route256.Practice.OrderService.DataAccess.Postgres;
using Ozon.Route256.Practice.OrderService.Handlers;
using Ozon.Route256.Practice.OrderService.Infrastructure;
using StackExchange.Redis;
using Ozon.Route256.Practice.OrderService.Application;
using Ozon.Route256.Practice.OrderService.Infrastructure.Database.Repositories;
using Ozon.Route256.Practice.OrderService.Infrastructure.Database.Mappers;

namespace Ozon.Route256.Practice.OrdersService
{
    public static class Startup
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection serviceCollection, IConfiguration configuration)
        {

            serviceCollection.AddGrpcClient<Customers.CustomersClient>(option =>
            {
                var url = configuration.GetValue<string>("CUSTOMER_SERVICE_ADDRESS");
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
            serviceCollection.AddScoped<IUnitOfWork, UnitOfWork>();
            serviceCollection.AddKafka().AddHandlers();
            serviceCollection.AddHostedService<SdConsumerHostedService>();
            serviceCollection.AddScoped<IOrderReadRepository, OrderReadRepository>();
            serviceCollection.AddScoped<IRegionsRepository, RegionReadRepository>();
            serviceCollection.AddScoped<IOrdersRepositoryPg, ShardOrdersDbAccess>();

            serviceCollection.AddScoped<ShardRegionsDbAccess>();
            serviceCollection.AddScoped<ICustomersRepositoryPg, ShardCustomerDbAccess>();
            serviceCollection.AddScoped<ICustomersRepository, RedisCustomerRepository>();
            serviceCollection.AddScoped<IDataReadMapper, DataLayerMapper>();
            serviceCollection.AddScoped<IDataWriteMapper, DataLayerMapper>();

            serviceCollection.Configure<KafkaConfiguration>(configuration.GetSection(nameof(KafkaConfiguration)));
            serviceCollection.AddSingleton<IConnectionMultiplexer>(
                _ =>
                {
                    var connection = ConnectionMultiplexer.Connect("redis", x => x.AbortOnConnectFail = false);

                    return connection;
                });
            var connectionString = configuration.GetConnectionString("OrderDatabase");
            PostgresMapping.MapCompositeTypes();

            serviceCollection.AddSingleton<IPostgresConnectionFactory>(_ => new PostgresConnectionFactory(connectionString));

            serviceCollection.Configure<DbOptions>(configuration.GetSection(nameof(DbOptions)));
            serviceCollection.AddSingleton<IShardPostgresConnectionFactory, ShardConnectionFactory>();
            serviceCollection.AddSingleton<IShardingRule<long>, LongShardingRule>();
            serviceCollection.AddSingleton<IShardingRule<string>, StringShardingRule>();
            serviceCollection.AddSingleton<IShardMigrator, ShardMigrator>();

            return serviceCollection;
        }
    }
}
