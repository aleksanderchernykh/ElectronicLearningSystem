using Confluent.Kafka;
using ElectronicLearningSystem.Infrastructure.Models.UserModel;
using ElectronicLearningSystem.Kafka.Common.Enums;
using ElectronicLearningSystem.Kafka.Core.Producer;
using ElectronicLearningSystemKafka.Common.Models;
using Microsoft.Extensions.Configuration;

namespace ElectronicLearningSystem.Common.Helpers.EmailSendingHelper
{
    /// <summary>
    /// Хелпер для работы с Email сообщениями.
    /// </summary>
    /// <param name="producer">Kafka producer. </param>
    /// <param name="configuration">Конфигурация приложения. </param>
    public class EmailSendingHelper(Producer producer,
        IConfiguration configuration) : IEmailSendingHelper
    {
        /// <summary>
        /// Kafka producer. 
        /// </summary>
        protected readonly Producer _producer = producer
            ?? throw new ArgumentException(nameof(_producer));

        /// <summary>
        /// Конфигурация приложения. 
        /// </summary>
        protected readonly IConfiguration _configuration = configuration
            ?? throw new ArgumentException(nameof(_configuration));

        /// <summary>
        /// Отправка сообщения для восстановления пароля пользователю.
        /// </summary>
        /// <param name="user">Пользователь. </param>
        /// <param name="token">Токен восстановления. </param>
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

        /// <summary>
        /// Отправка сообщения.
        /// </summary>
        /// <param name="email">Данные для сообщения.</param>
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
