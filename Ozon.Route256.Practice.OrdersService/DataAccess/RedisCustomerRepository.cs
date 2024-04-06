using System.Text.Json;
using System.Text.Json.Serialization;
using StackExchange.Redis;

namespace Ozon.Route256.Practice.OrdersService.DataAccess;

public class RedisCustomerRepository : ICustomersRepository
{
    private readonly JsonSerializerOptions _jsonSerializerOptions = new()
    {
        Converters =
        {
            new JsonStringEnumConverter()
        }
    };

    private readonly IDatabase _database;

    public RedisCustomerRepository(IConnectionMultiplexer connectionMultiplexer)
    {
        _database = connectionMultiplexer.GetDatabase(0);
    }

    public async Task<Customer?> Find(long customerId, CancellationToken token)
    {
        if (token.IsCancellationRequested)
        {
            return null;
        }

        var key = BuildOrderKey(customerId);
        var resultRedis = await _database.StringGetAsync(key);

        var result = resultRedis.HasValue ? JsonSerializer.Deserialize<Customer>(resultRedis.ToString(), _jsonSerializerOptions) : (Customer?)null;

        return result;
    }

    public async Task Insert(Customer customer, CancellationToken token)
    {
        if (token.IsCancellationRequested)
        {
            return;
        }

        var key = BuildOrderKey(customer.Id);

        if (_database.KeyExists(key))
        {
            throw new Exception($"Customer with id {customer.Id} already exists");
        }

        var resultRedis = JsonSerializer.Serialize(customer, _jsonSerializerOptions);

        await _database.StringSetAsync(key, resultRedis);
    }

    private static RedisKey BuildOrderKey(long customerId)
    {
        return new RedisKey($"customers:{customerId}");
    }
}