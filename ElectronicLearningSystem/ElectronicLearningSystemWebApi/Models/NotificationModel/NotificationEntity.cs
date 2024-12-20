using ElectronicLearningSystemWebApi.Models.NotificationTypeModel;
using ElectronicLearningSystemWebApi.Models.UserModel;

namespace ElectronicLearningSystemWebApi.Models.NotificationModel
{
    public class NotificationEntity: EntityBase
    {
        public required string Text { get; set; }

        public bool IsReady { get; set; }

        public required UserEntity Recipient { get; set; }
        public Guid RecipientId { get; set; }

        public required NotificationTypeEntity NotificationType {  get; set; }
        public Guid NotificationTypeId { get; set; }
    }
}
