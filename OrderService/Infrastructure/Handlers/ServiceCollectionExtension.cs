using Ozon.Route256.Practice.OrderService.Handlers.OrderRegistration;

namespace Ozon.Route256.Practice.OrderService.Handlers;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddHandlers(this IServiceCollection collection)
    {
        collection.AddScoped<IOrderRegistrationHandler, OrderRegistrationHandler>();
        collection.AddScoped<IOrderEventHandler, OrderEventHandler>();
        return collection;
    }
}