using Confluent.Kafka;

namespace Ozon.Route256.Practice.OrdersService.Infrastructure.Kafka.Consumers;
public abstract class ConsumerBackgroundService<TKey, TValue> : BackgroundService
{
    private readonly IKafkaDataConsumer<TKey, TValue> _kafkaDataProvider;
    private readonly ILogger _logger;
    protected readonly IServiceScope _scope;

    protected Dictionary<string, IKafkaConsumeHandler<TKey, TValue>> Consumers { get; set; }
    protected ConsumerBackgroundService(
        IServiceProvider serviceProvider,
        IKafkaDataConsumer<TKey, TValue> kafkaDataProvider,
        ILogger logger)
    {
        _kafkaDataProvider = kafkaDataProvider;
        _logger = logger;
        _scope = serviceProvider.CreateScope();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.Yield();

        if (stoppingToken.IsCancellationRequested)
        {
            return;
        }

        _kafkaDataProvider.Consumer.Subscribe(Consumers.Keys);
        _logger.LogInformation("Start consumer topic {Topic}", String.Join(',',  Consumers.Keys)) ;

        while (!stoppingToken.IsCancellationRequested)
        {
            await ConsumeAsync(stoppingToken);
        }

        _kafkaDataProvider.Consumer.Unsubscribe();

        _logger.LogInformation("Stop consumer topics {Topics}", String.Join(',', Consumers.Keys));
    }

    private async Task ConsumeAsync(CancellationToken cancellationToken)
    {
        ConsumeResult<TKey, TValue>? message = null;

        try
        {
            message = _kafkaDataProvider.Consumer.Consume(TimeSpan.FromMilliseconds(100));

            if (message is null)
            {
                //_logger.LogWarning("Message is null");
                await Task.Delay(100, cancellationToken);
                return;
            }

            _logger.LogInformation("message {Message} received from topic {Topic}", message.Message.Value, message.Topic);

            if (Consumers.ContainsKey(message.Topic))
                await Consumers[message.Topic].HandleAsync(message, cancellationToken);
            else
                _logger.LogWarning("no handler for topic {Topic}", message.Topic);
            
            _kafkaDataProvider.Consumer.Commit();
        }
        catch (Exception exc)
        {
            var key = message is not null ? message.Message.Key!.ToString() : "No key";
            var value = message is not null ? message.Message.Value!.ToString() : "No value";

            _logger.LogError(exc, "Error process message Key:{Key} Value:{Value}", key, value);
        }
    }
    public override void Dispose()
    {
        _scope.Dispose();
        base.Dispose();
    }
}