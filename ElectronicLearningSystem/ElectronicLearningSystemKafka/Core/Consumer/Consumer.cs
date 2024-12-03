using Avro.Specific;
using Confluent.Kafka;
using Confluent.SchemaRegistry.Serdes;
using Confluent.SchemaRegistry;
using ElectronicLearningSystemCore.Extensions;
using ElectronicLearningSystemKafka.Common.Enums;
using Microsoft.Extensions.Logging;
using Confluent.Kafka.SyncOverAsync;
using System.Threading;

namespace ElectronicLearningSystemKafka.Core.Consumer
{
    public sealed class Consumer : IDisposable
    {
        private readonly ConsumerConfig _consumerConfig;
        private readonly ILogger _logger;
        private readonly CachedSchemaRegistryClient _schemaRegistryClient;
        private readonly CancellationTokenSource _cts = new();

        public Consumer(ILogger<Consumer> logger, string bootstrapServersUrl, string schemaRegistryUrl)
        {
            if (string.IsNullOrWhiteSpace(bootstrapServersUrl))
            {
                throw new ArgumentNullException(nameof(bootstrapServersUrl));
            }

            if (string.IsNullOrWhiteSpace(schemaRegistryUrl))
            {
                throw new ArgumentNullException(nameof(schemaRegistryUrl));
            }

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
        public void SubscribeTopic<TKey, TValue>(string groupId, TopicEnum topic, Action<ConsumeResult<TKey, TValue>> action) 
            where TValue : ISpecificRecord
        {
            SubscribeTopic(groupId, topic.GetAmbientValue().ToString(), action);
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
        public void SubscribeTopic<TKey, TValue>(string groupId, string topic, Action<ConsumeResult<TKey, TValue>> action)
            where TValue : ISpecificRecord
        {
            if (string.IsNullOrWhiteSpace(topic))
            {
                throw new ArgumentNullException(nameof(topic));
            }

            _consumerConfig.GroupId = groupId ?? throw new ArgumentNullException(nameof(groupId));

            var avroDeserializer = new AvroDeserializer<TValue>(_schemaRegistryClient)
                .AsSyncOverAsync();

            using var consumerBuilder = new ConsumerBuilder<TKey, TValue>(_consumerConfig)
                .SetValueDeserializer(avroDeserializer)
                .Build();
            consumerBuilder.Subscribe(topic);

            try
            {
                while (!_cts.Token.IsCancellationRequested)
                {
                    try
                    {
                        var consumeResult = consumerBuilder.Consume(_cts.Token);
                        action?.Invoke(consumeResult);
                        _logger.LogDebug($"Message: {consumeResult.Message.Value}, Partition: {consumeResult.Partition}, Offset: {consumeResult.Offset}");
                    }
                    catch (ConsumeException e)
                    {
                        _logger.LogError($"Error occurred: {e.Error.Reason}");
                    }
                }
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("Consumer loop canceled.");
            }
            finally
            {
                consumerBuilder.Close();
            }
        }

        /// <summary>
        /// Отписка от топика.
        /// </summary>
        public void UnsubscribeTopic()
        {
            if (_cts != null && !_cts.IsCancellationRequested)
            {
                _cts.Cancel();
            }
        }

        public void Dispose()
        {
            UnsubscribeTopic();
        }
    }
}
