﻿using FluentMigrator.Runner;
using Grpc.Core;
using Microsoft.Extensions.Options;
using Npgsql;
using Ozon.Route256.Practice.OrderService.Dal.Common.Shard;
using Ozon.Route256.Practice.OrderService.ClientBalancing;
using GrpcReplicaType = Ozon.Route256.Practice.Replica.Types.ReplicaType;

namespace Ozon.Route256.Practice.OrderService.Dal.Common.Shard;

public interface IShardMigrator
{
    Task Migrate(
        CancellationToken token);
}

public class ShardMigrator: IShardMigrator
{
    private readonly DbOptions _dbOptions;
    private readonly SdService.SdServiceClient _client;

    public ShardMigrator(
        IOptions<DbOptions> dbOptions,
        SdService.SdServiceClient client)
    {
        _dbOptions   = dbOptions.Value;
        _client = client;
    }


    public async Task Migrate(
        CancellationToken token)
    {
         var endpoints = await GetEndpoints(token);

         foreach (var endpoint in endpoints)
         {
             var connectionString = GetConnectionString(endpoint);
             foreach (var bucketId in endpoint.Buckets)
             {
                 var serviceProvider = CreateServices(connectionString);
                 using var scope = serviceProvider.CreateScope();
                 var context = scope.ServiceProvider.GetRequiredService<BucketMigrationContext>();
                 context.UpdateCurrentDbSchema(bucketId);
                 var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
                 runner.MigrateUp();
                //runner.MigrateDown(0);
             }
         }
    }

    private static IServiceProvider CreateServices(
        string connectionString)
    {
        return new ServiceCollection()
            .AddSingleton<BucketMigrationContext>()
            .AddFluentMigratorCore()
            .AddLogging(o => o.AddFluentMigratorConsole())
            .ConfigureRunner(builder => builder
                .AddPostgres()
                .WithGlobalConnectionString(connectionString)
                .WithMigrationsIn(typeof(SqlMigration).Assembly)
                .ScanIn(typeof(ShardVersionTableMetaData).Assembly).For.VersionTableMetaData()
            )
            .BuildServiceProvider();
    }
    
    private string GetConnectionString(
        DbEndpoint endpoint)
    {
        var builder = new NpgsqlConnectionStringBuilder
        {
            Host = endpoint.HostAndPort,
            Database = _dbOptions.DatabaseName,
            Username = _dbOptions.User,
            Password = _dbOptions.Password,
            IncludeErrorDetail = true
        };
        return builder.ToString();
    }

    private async Task<DbEndpoint[]> GetEndpoints(CancellationToken token)
    {
        var request = new DbResourcesRequest
        {
            ClusterName = _dbOptions.ClusterName
        };

        using var stream = 
            _client.DbResources(request, cancellationToken: token);

        await stream.ResponseStream.MoveNext(token);
        var response = stream.ResponseStream.Current;
        return GetEndpoints(response).ToArray();
    }
    
    private static IEnumerable<DbEndpoint> GetEndpoints(DbResourcesResponse response) =>
        response.Replicas.Select(replica => new DbEndpoint(
            $"{replica.Host}:{replica.Port}",
            ToDbReplica(replica.Type),
            replica.Buckets.ToArray()));
    
    private static DbReplicaType ToDbReplica(GrpcReplicaType replicaType) =>
        replicaType switch
        {
            GrpcReplicaType.Master => DbReplicaType.Master,
            GrpcReplicaType.Async  => DbReplicaType.Async,
            GrpcReplicaType.Sync   => DbReplicaType.Sync,
            _                      => throw new ArgumentOutOfRangeException(nameof(replicaType), replicaType, null)
        };
}