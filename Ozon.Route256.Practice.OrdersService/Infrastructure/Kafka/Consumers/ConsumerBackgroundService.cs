﻿using Confluent.Kafka;

namespace Ozon.Route256.Practice.OrdersService.Infrastructure.Kafka.Consumers;
public abstract class ConsumerBackgroundService<TKey, TValue> : BackgroundService
{
    private readonly IKafkaDataProvider<TKey, TValue> _kafkaDataProvider;
    private readonly ILogger _logger;
    private readonly string _topicName;
    protected readonly IServiceScope _scope;

    protected ConsumerBackgroundService(
        string topicName,
        IServiceProvider serviceProvider,
        IKafkaDataProvider<TKey, TValue> kafkaDataProvider,
        ILogger logger)
    {
        _topicName = topicName;
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

        _kafkaDataProvider.Consumer.Subscribe(_topicName);

        _logger.LogInformation("Start consumer topic {Topic}", _topicName);

        while (!stoppingToken.IsCancellationRequested)
        {
            await ConsumeAsync(stoppingToken);
        }

        _kafkaDataProvider.Consumer.Unsubscribe();

        _logger.LogInformation("Stop consumer topic {Topic}", _topicName);
    }

    private async Task ConsumeAsync(CancellationToken cancellationToken)
    {
        ConsumeResult<TKey, TValue>? message = null;

        try
        {
            message = _kafkaDataProvider.Consumer.Consume(TimeSpan.FromMilliseconds(100));

            if (message is null)
            {
                await Task.Delay(100, cancellationToken);
                return;
            }

            await HandleAsync(message, cancellationToken);
            _kafkaDataProvider.Consumer.Commit();
        }
        catch (Exception exc)
        {
            var key = message is not null ? message.Message.Key!.ToString() : "No key";
            var value = message is not null ? message.Message.Value!.ToString() : "No value";

            _logger.LogError(exc, "Error process message Key:{Key} Value:{Value}", key, value);
        }
    }

    protected abstract Task HandleAsync(ConsumeResult<TKey, TValue> message, CancellationToken cancellationToken);

    public override void Dispose()
    {
        _scope.Dispose();
        base.Dispose();
    }
}