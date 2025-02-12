namespace ElectronicLearningSystemWebApi.Models.NotificationModel.Response
{
    public class NotificationResponse : BaseResponse
    {
        public required string Text { get; set; }

        public bool IsReady { get; set; }

        public Guid RecipientId { get; set; }

        public Guid NotificationTypeId { get; set; }
    }
}
