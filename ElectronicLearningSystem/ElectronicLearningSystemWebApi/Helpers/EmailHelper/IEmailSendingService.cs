using ElectronicLearningSystemKafka.Common.Models;

namespace ElectronicLearningSystemWebApi.Helpers.EmailHelper
{
    public interface IEmailSendingService
    {
        Task SendEmailAsync(Email email);
    }
}
