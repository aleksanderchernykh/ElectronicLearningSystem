using ElectronicLearningSystemKafka.Common.Models;
using ElectronicLearningSystemWebApi.Models.UserModel.Entity;

namespace ElectronicLearningSystemWebApi.Helpers.EmailSendingHelper
{
    public interface IEmailSendingHelper
    {
        Task SendRecoveryPasswordAsync(UserEntity user, string token);

        Task SendEmailAsync(Email email);
    }
}
