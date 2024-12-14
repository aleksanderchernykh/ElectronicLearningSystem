using ElectronicLearningSystemKafka.Common.Enums;
using ElectronicLearningSystemKafka.Common.Models;
using ElectronicLearningSystemKafka.Core.Consumer;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace EmailSendingService
{
    public class KafkaConsumerWorker(ILogger<KafkaConsumerWorker> logger,
        Consumer consumer,
        EmailSender sender) : BackgroundService
    {
        private readonly ILogger<KafkaConsumerWorker> _logger = 
            logger ?? throw new ArgumentNullException(nameof(logger));

        private readonly Consumer _consumer = 
            consumer ?? throw new ArgumentNullException(nameof(consumer));

        private readonly EmailSender _sender = 
            sender ?? throw new ArgumentNullException(nameof(sender));

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                await SubscribeTopic(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ошибка подписки на топик: {ex.Message}");
            }
        }

        protected virtual async Task SubscribeTopic(CancellationToken stoppingToken)
        {
            await _consumer.SubscribeTopic<string, Email>(
                "emailreader",
                TopicEnum.EmailSending,
                topic => _sender.SendEmailAsync(topic.Message.Value),
                stoppingToken
            );
        }
    }
}
