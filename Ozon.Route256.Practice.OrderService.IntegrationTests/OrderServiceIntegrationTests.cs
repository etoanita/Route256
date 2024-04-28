using FluentAssertions;

namespace Ozon.Route256.Practice.OrderService.IntegrationTests;

public class OrderServiceIntegrationTests : IClassFixture<OrderServiceAppFactory>
{
    private readonly OrderServiceAppFactory _appFactory;

    public OrderServiceIntegrationTests(OrderServiceAppFactory appFactory)
        => _appFactory = appFactory;

    [Fact]
    public async Task GetRegionsList()
    {
        var client = _appFactory.OrdersClient;

        var response = await client.GetRegionsListAsync(new GetRegionsListRequest());

        response.Should().NotBeNull();
    }
}
