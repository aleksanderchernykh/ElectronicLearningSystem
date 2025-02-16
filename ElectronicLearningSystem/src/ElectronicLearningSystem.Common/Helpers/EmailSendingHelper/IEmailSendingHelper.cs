using ElectronicLearningSystem.Infrastructure.Models.UserModel;
using ElectronicLearningSystemKafka.Common.Models;

namespace ElectronicLearningSystem.Common.Helpers.EmailSendingHelper
{
    public interface IEmailSendingHelper
    {
        Task SendRecoveryPasswordAsync(UserEntity user, string token);

        Task SendEmailAsync(Email email);
    }
}
