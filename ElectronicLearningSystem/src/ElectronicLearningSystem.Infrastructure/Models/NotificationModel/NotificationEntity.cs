using ElectronicLearningSystem.Infrastructure.Models.NotificationTypeModel;
using ElectronicLearningSystem.Infrastructure.Models.UserModel;

namespace ElectronicLearningSystem.Infrastructure.Models.NotificationModel
{
    public class NotificationEntity : EntityBase
    {
        public required string Text { get; set; }

        public bool IsReady { get; set; }

        public UserEntity Recipient { get; set; }
        public Guid RecipientId { get; set; }

        public NotificationTypeEntity NotificationType { get; set; }
        public Guid NotificationTypeId { get; set; }
    }
}
