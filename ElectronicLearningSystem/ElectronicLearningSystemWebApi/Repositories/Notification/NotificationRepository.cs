using ElectronicLearningSystemWebApi.Context;
using ElectronicLearningSystemWebApi.Helpers;
using ElectronicLearningSystemWebApi.Models.NotificationModel;
using ElectronicLearningSystemWebApi.Repositories.Base;

namespace ElectronicLearningSystemWebApi.Repositories.Notification
{
    public class NotificationRepository(ApplicationContext context, UserHelper userHelper)
        : RepositoryBase<NotificationEntity>(context), INotificationRepository
    {
        protected readonly UserHelper _userHelper = userHelper
            ?? throw new ArgumentNullException(nameof(userHelper));

        public async Task<IList<NotificationEntity>> GetActualNotificationByCurrentUserAsync()
        {
            return await GetRecordsByQueryAsync(x => x.IsReady == false 
                && x.RecipientId == userHelper.GetCurrentUserId());
        }
    }
}
