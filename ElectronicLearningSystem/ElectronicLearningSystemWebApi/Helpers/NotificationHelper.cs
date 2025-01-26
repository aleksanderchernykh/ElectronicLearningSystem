using ElectronicLearningSystemWebApi.Models.NotificationModel;
using ElectronicLearningSystemWebApi.Models.NotificationModel.DTO;

namespace ElectronicLearningSystemWebApi.Helpers
{
    public class NotificationHelper(UserHelper userHelper)
    {
        protected readonly UserHelper _userHelper = userHelper
            ?? throw new ArgumentNullException(nameof(userHelper));

        public virtual NotificationEntity GetNotificationByDTO(CreateNotificationDTO createNotificationDTO)
        {
            return new NotificationEntity
            {
                Text = createNotificationDTO.Text,
                RecipientId = createNotificationDTO.Recipient,
                NotificationTypeId = createNotificationDTO.NotificationType,
                CreatedOn = DateTime.UtcNow,
                ModifiedOn = DateTime.UtcNow,
                CreatedById = _userHelper.GetCurrentUserId(),
                ModifiedById = _userHelper.GetCurrentUserId()
            };
        }
    }
}
