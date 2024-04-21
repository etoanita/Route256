namespace Ozon.Route256.Practice.OrderService.ClientBalancing;

public sealed record DbEndpoint(
    string HostAndPort, 
    DbReplicaType DbReplica,
    int[] Buckets);