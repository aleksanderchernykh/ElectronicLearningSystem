using Confluent.Kafka;
using ElectronicLearningSystemKafka.Common.Enums;
using ElectronicLearningSystemKafka.Common.Models;
using ElectronicLearningSystemKafka.Core.Producer;
using ElectronicLearningSystemWebApi.Models.UserModel;

namespace ElectronicLearningSystemWebApi.Helpers.Controller
{
    public class EmailSendingHelper(Producer producer, IConfiguration configuration)
    {
        protected readonly Producer _producer = producer
            ?? throw new ArgumentException(nameof(_producer));

        protected readonly IConfiguration _configuration = configuration
            ?? throw new ArgumentException(nameof(_configuration));

        public virtual async Task SendRecoveryPasswordAsync(UserEntity user, string token)
        {
            var emailMessage = new Email
            {
                Recipients = new List<string>
                {
                    user.Email
                },
                Subject = "Password reset",
                Text = $@"
                <html>
                    <body>
                        <h4>Hello, {user.LastName} {user.FirstName}</h4>
                        <p><h5>Link to restore password: {configuration["UI:ConnectionString"]}/forgot-password?token={token}</h5></p>
                    </body>
                </html>"
            };

            await SendEmailAsync(emailMessage);
        }

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
