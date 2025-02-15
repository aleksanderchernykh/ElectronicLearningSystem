namespace ElectronicLearningSystem.Application.Models.NotificationModel.DTO
{
    public class CreateNotificationDTO
    {
        public required Guid RecipientId { get; set; }

        public required Guid NotificationTypeId { get; set; }

        public required string Text { get; set; }
    }
}
