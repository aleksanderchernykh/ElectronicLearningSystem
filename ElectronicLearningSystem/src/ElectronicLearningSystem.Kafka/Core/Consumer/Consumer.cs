using ElectronicLearningSystem.Core.Extensions;
using Confluent.Kafka;
using Confluent.SchemaRegistry;
using ElectronicLearningSystem.Kafka.Common.Enums;
using Microsoft.Extensions.Logging;
using Avro.Specific;
using Confluent.SchemaRegistry.Serdes;
using Confluent.Kafka.SyncOverAsync;

namespace ElectronicLearningSystem.Kafka.Core.Consumer
{
    public sealed class Consumer
    {
        private readonly ConsumerConfig _consumerConfig;
        private readonly ILogger<Consumer> _logger;
        private readonly CachedSchemaRegistryClient _schemaRegistryClient;

        public Consumer(ILogger<Consumer> logger, string bootstrapServersUrl, string schemaRegistryUrl)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(bootstrapServersUrl);
            ArgumentException.ThrowIfNullOrWhiteSpace(schemaRegistryUrl);

            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _consumerConfig = new ConsumerConfig
            {
                BootstrapServers = bootstrapServersUrl,
                AutoOffsetReset = AutoOffsetReset.Latest,
                EnableAutoCommit = true
            };
            _schemaRegistryClient = new CachedSchemaRegistryClient(new SchemaRegistryConfig
            {
                Url = schemaRegistryUrl
            });
        }

        /// <summary>
        /// Подписка на обработчик топика.
        /// </summary>
        /// <typeparam name="TKey">Ключ сообщения.</typeparam>
        /// <typeparam name="TValue">Значение сообщения.</typeparam>
        /// <param name="groupId">Группа обработки.</param>
        /// <param name="topic">Топик для обработки.</param>
        /// <param name="action">Действие, выполняемое для полученного сообщения.</param>
        /// <exception cref="ArgumentNullException">Передано пустое значение.</exception>
        public async Task SubscribeTopic<TKey, TValue>(string groupId, TopicEnum topic, Action<ConsumeResult<TKey, TValue>> action, CancellationToken cancellationToken)
            where TValue : ISpecificRecord
        {
            await SubscribeTopic(groupId, topic.GetAmbientValue().ToString(), action, cancellationToken);
        }

        /// <summary>
        /// Подписка на обработчик топика.
        /// </summary>
        /// <typeparam name="TKey">Ключ сообщения.</typeparam>
        /// <typeparam name="TValue">Значение сообщения.</typeparam>
        /// <param name="groupId">Группа обработки.</param>
        /// <param name="topic">Топик для обработки.</param>
        /// <param name="action">Действие, выполняемое для полученного сообщения.</param>
        /// <exception cref="ArgumentNullException">Передано пустое значение.</exception>
        public async Task SubscribeTopic<TKey, TValue>(string groupId, string topic, Action<ConsumeResult<TKey, TValue>> action, CancellationToken cancellationToken)
            where TValue : ISpecificRecord
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(topic);

            _consumerConfig.GroupId = groupId ?? throw new ArgumentNullException(nameof(groupId));

            var avroDeserializer = new AvroDeserializer<TValue>(_schemaRegistryClient)
                .AsSyncOverAsync();

            var consumerBuilder = new ConsumerBuilder<TKey, TValue>(_consumerConfig)
                .SetValueDeserializer(avroDeserializer)
                .Build();
            consumerBuilder.Subscribe(topic);

            try
            {
                await Task.Run(() =>
                {
                    while (!cancellationToken.IsCancellationRequested)
                    {
                        try
                        {
                            var consumeResult = consumerBuilder.Consume(cancellationToken);
                            action?.Invoke(consumeResult);
                            _logger.LogDebug($"Message: {consumeResult.Message.Value}, Partition: {consumeResult.Partition}, Offset: {consumeResult.Offset}");
                        }
                        catch (OperationCanceledException e)
                        {
                            _logger.LogError($"Email consumer is canceled");
                        }
                        catch (ConsumeException e)
                        {
                            _logger.LogError($"Error occurred: {e.Error.Reason}");
                        }
                    }
                }, cancellationToken);
            }
            finally
            {
                consumerBuilder.Close();
            }
        }
    }
}
