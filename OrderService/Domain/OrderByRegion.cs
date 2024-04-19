using Ozon.Route256.Practice.OrderService.Domain.Core;

namespace Ozon.Route256.Practice.OrderService.Domain
{
    public class OrderByRegion : ValueObject
    {
        public string Region { get; }
        public int OrdersCount { get; }
        public double TotalPrice { get; }
        public long TotalWeight { get; }
        public int ClientsCount { get; }

        private OrderByRegion(string region, int ordersCount, double totalPrice, long totalWeight, int clientsCount)
        {
            Region = region;
            OrdersCount = ordersCount;
            TotalPrice = totalPrice;
            TotalWeight = totalWeight;
            ClientsCount = clientsCount;
        }

        public static OrderByRegion CreateInstance(string region, int ordersCount, double totalPrice, long totalWeight, int clientsCount)
        {
            // TODO: Validate
            return new OrderByRegion(region, ordersCount, totalPrice, totalWeight, clientsCount);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Region;
            yield return OrdersCount;
            yield return TotalPrice;
            yield return TotalWeight;
            yield return ClientsCount;
        }
    }
}
