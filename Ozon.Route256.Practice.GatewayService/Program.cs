using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Ozon.Route256.Practice.OrdersService;
using System.Net;

await Host
    .CreateDefaultBuilder(args)
    .ConfigureWebHostDefaults(builder => builder.UseStartup<Startup>())
    .Build()
    .RunAsync();

public static class ProgramExtension
{
    public const string ROUTE256_GRPC_PORT = "ROUTE256_GRPC_PORT";
    public const string ROUTE256_HTTP_PORT = "ROUTE256_HTTP_PORT";

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
}