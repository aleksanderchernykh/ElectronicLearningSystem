using ElectronicLearningSystemKafka.Common.Enums;
using ElectronicLearningSystemKafka.Common.Models;
using ElectronicLearningSystemKafka.Core.Consumer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace EmailSendingService
{
    public class KafkaConsumerWorker : BackgroundService
    {
        private readonly ILogger<Consumer> _logger;
        private readonly IConfiguration _configuration;
        private readonly Consumer _consumer;

        public KafkaConsumerWorker(ILogger<Consumer> logger, IConfiguration configuration)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));

            _consumer = new Consumer(
                _logger,
                _configuration["Kafka:KafkaBrokerUrl"] ?? throw new ArgumentNullException("Не заполнено значение Kafka:KafkaBrokerUrl"),
                _configuration["Kafka:SchemaRegistryUrl"] ?? throw new ArgumentNullException("Не заполнено значение Kafka:SchemaRegistryUrl")
            );
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                await SubscribeTopic();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ошибка подписки на топик: {ex.Message}");
            }
        }

        protected virtual async Task SubscribeTopic()
        {
            await _consumer.SubscribeTopic<string, Email>(
                "emailreader",
                TopicEnum.EmailSending,
                topic => _logger.LogInformation($"Заголовок: {topic.Message.Value.Subject}")
            );
        }
    }
}
