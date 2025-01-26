namespace ElectronicLearningSystemWebApi.Models.NotificationModel.DTO
{
    public class CreateNotificationDTO
    {
        public required Guid Recipient {  get; set; }

        public required Guid NotificationType { get; set; }

        public required string Text { get; set; }
    }
}
