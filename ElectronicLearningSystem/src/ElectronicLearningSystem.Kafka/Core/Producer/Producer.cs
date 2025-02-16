using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Avro.Specific;
using Confluent.SchemaRegistry;
using Confluent.SchemaRegistry.Serdes;
using ElectronicLearningSystem.Core.Extensions;
using ElectronicLearningSystem.Kafka.Common.Enums;

namespace ElectronicLearningSystem.Kafka.Core.Producer
{
    public sealed class Producer
    {
        private readonly ProducerConfig _producerConfig;
        private readonly SchemaRegistryConfig _schemaRegistryConfig;
        private readonly ILogger<Producer> _logger;

        /// <summary>
        /// ctor.
        /// </summary>
        /// <param name="logger">Логгер.</param>
        /// <param name="bootstrapServersUrl">Url Kafka Broker</param>
        /// <param name="schemaRegistryUrl">Url SchemaRegistry.</param>
        /// <exception cref="ArgumentNullException">Пустое значение bootstrapServersUrl или schemaRegistryUrl.</exception>
        public Producer(ILogger<Producer> logger, string bootstrapServersUrl, string schemaRegistryUrl)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(bootstrapServersUrl);
            ArgumentException.ThrowIfNullOrWhiteSpace(schemaRegistryUrl);

            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _producerConfig = new ProducerConfig
            {
                BootstrapServers = bootstrapServersUrl,
                Acks = Acks.All,
                AllowAutoCreateTopics = false
            };
            _schemaRegistryConfig = new SchemaRegistryConfig
            {
                Url = schemaRegistryUrl
            };
        }

        /// <summary>
        /// Отправка сообщения.
        /// </summary>
        /// <typeparam name="TKey">Ключ сообщения.</typeparam>
        /// <typeparam name="TValue">Значение сообщения.</typeparam>
        /// <param name="topic">Топик.</param>
        /// <param name="message">Сообщение.</param>
        public async Task SendMessage<TKey, TValue>(TopicEnum topic, Message<TKey, TValue> message)
            where TValue : ISpecificRecord
        {
            using var schemaRegistry = new CachedSchemaRegistryClient(_schemaRegistryConfig);

            using var producer = new ProducerBuilder<TKey, TValue>(_producerConfig)
                .SetValueSerializer(new AvroSerializer<TValue>(schemaRegistry))
                .Build();

            try
            {
                var result = await producer.ProduceAsync(topic.GetAmbientValue().ToString(), message);
                _logger.LogDebug($"Сообщение {result.Message} успешно доставлено в топик {result.Topic}");
            }
            catch (ProduceException<TKey, TValue> e)
            {
                _logger.LogError($"Во время отправки сообщения произошла ошибка {e.Error}");
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
            }
        }
    }
}