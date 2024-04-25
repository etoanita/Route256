using FluentMigrator.Runner;
using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Ozon.Route256.Practice.OrdersService;
using Ozon.Route256.Practice.OrderService.Dal.Common.Shard;
using System.Net;

 await Host
    .CreateDefaultBuilder(args)
    .ConfigureWebHostDefaults(builder => builder.UseStartup<Startup>()
        .ConfigureKestrel(option =>
        {
            option.ListenPortByOptions(ProgramExtension.ROUTE256_GRPC_PORT, HttpProtocols.Http2);
        }))
    .Build()
    .RunOrMigrate(args);

public static class ProgramExtension
{
    public const string ROUTE256_GRPC_PORT = "ROUTE256_GRPC_PORT";

    public static void ListenPortByOptions(
        this KestrelServerOptions option,
        string envOption,
        HttpProtocols httpProtocol)
    {
        var isHttpPortParsed = int.TryParse(Environment.GetEnvironmentVariable(envOption), out var httpPort);

        if (isHttpPortParsed)
        {
            option.ListenAnyIP(httpPort, options => options.Protocols = httpProtocol);
        }
    }

    public static async Task RunOrMigrate(
        this IHost host,
        string[] args)
    {
        var needMigration = args.Length > 0 && args[0].Equals("migrate", StringComparison.InvariantCultureIgnoreCase);
        if (needMigration)
        {
            using var cts = new CancellationTokenSource(TimeSpan.FromMinutes(1));
            using var scope = host.Services.CreateScope();
            var runner = scope.ServiceProvider.GetRequiredService<IShardMigrator>();
            await runner.Migrate(cts.Token);
        }
        else
            await host.RunAsync();
    }
}