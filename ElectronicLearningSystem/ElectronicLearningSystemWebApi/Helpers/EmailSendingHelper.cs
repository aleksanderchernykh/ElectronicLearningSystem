using Confluent.Kafka;
using ElectronicLearningSystemKafka.Common.Enums;
using ElectronicLearningSystemKafka.Common.Models;
using ElectronicLearningSystemKafka.Core.Producer;

namespace ElectronicLearningSystemWebApi.Helpers
{
    public class EmailSendingHelper(Producer producer)
    {
        protected readonly Producer _producer = producer;

        public virtual async Task SendEmailAsync(Email email)
        {
            var message = new Message<string, Email>
            {
                Key = Guid.NewGuid().ToString(),
                Value = email
            };

            await _producer.SendMessage(TopicEnum.EmailSending, message);
        }
    }
}
