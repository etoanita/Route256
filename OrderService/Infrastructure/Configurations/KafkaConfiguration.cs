namespace Ozon.Route256.Practice.OrderService.Configurations
{
    public class KafkaConfiguration
    {
        public string Brokers { get; set; }
        public Topics Topics { get; set; }
        public string ConsumerGroup { get; set; }
    }

    public class Topics
    {
        public string NewOrderTopic { get; set; }
        public string OrderTopic { get; set; }
    }
}
