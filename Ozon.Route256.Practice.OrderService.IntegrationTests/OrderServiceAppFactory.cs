using Grpc.Net.Client;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Ozon.Route256.Practice.OrdersService;

namespace Ozon.Route256.Practice.OrderService.IntegrationTests;

public sealed class OrderServiceAppFactory : WebApplicationFactory<Startup>
{
    public Orders.OrdersClient OrdersClient { get; }

    public OrderServiceAppFactory()
    {
        var hostBuilder = CreateHostBuilder();
        hostBuilder.ConfigureWebHost(builder => builder.UseTestServer());

        var client = CreateClient();
        var grpcChannel = GrpcChannel.ForAddress(client.BaseAddress!,
            new GrpcChannelOptions
            {
                HttpClient = client,
            });

        OrdersClient = new Orders.OrdersClient(grpcChannel);
    }

    /// <inheritdoc />
    protected override IHostBuilder CreateHostBuilder()
    {
        var memoryConfig = new MemoryConfigurationSource
        {
            InitialData = new[]
            {
                new KeyValuePair<string, string>("ROUTE256_SD_ADDRESS", "http://localhost:6081"),
                new KeyValuePair<string, string>("CUSTOMER_SERVICE_ADDRESS", "http://localhost:6082"),
                new KeyValuePair<string, string>("LOGISTICS_SIMULATOR_ADDRESS", "http://localhost:6090"),
            }!
        };

        var builder = Host.CreateDefaultBuilder()
            .ConfigureAppConfiguration(cfg => cfg.Add(memoryConfig))
            .ConfigureServices(services
                => services.AddGrpcClient<Orders.OrdersClient>(o => o.Address = new Uri("https://localhost:5010")))
            .ConfigureWebHostDefaults(hostBuilder => hostBuilder.UseStartup<Startup>());

        return builder;
    }
}