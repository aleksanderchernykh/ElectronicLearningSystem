using ElectronicLearningSystemWebApi.Context;
using ElectronicLearningSystemWebApi.Models.NotificationModel.Entity;
using ElectronicLearningSystemWebApi.Repositories.Base;
using ElectronicLearningSystemWebApi.Services.UserService;

namespace ElectronicLearningSystemWebApi.Repositories.Notification
{
    public class NotificationRepository(ApplicationContext context, UserService userHelper)
        : RepositoryBase<NotificationEntity>(context), INotificationRepository
    {
        protected readonly UserService _userHelper = userHelper
            ?? throw new ArgumentNullException(nameof(userHelper));

        public async Task<IList<NotificationEntity>> GetActualNotificationByCurrentUserAsync()
        {
            return await GetRecordsByQueryAsync(x => x.IsReady == false 
                && x.RecipientId == userHelper.GetCurrentUserId());
        }
    }
}
