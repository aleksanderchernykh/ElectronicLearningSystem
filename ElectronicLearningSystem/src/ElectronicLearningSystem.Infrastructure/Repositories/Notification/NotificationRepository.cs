using ElectronicLearningSystem.Core.Helpers;
using ElectronicLearningSystem.Infrastructure.Context;
using ElectronicLearningSystem.Infrastructure.Models.NotificationModel;
using ElectronicLearningSystem.Infrastructure.Repositories.Base;

namespace ElectronicLearningSystem.Infrastructure.Repositories.Notification
{
    public class NotificationRepository(ApplicationContext context, IUserHelper userHelper)
        : RepositoryBase<NotificationEntity>(context), INotificationRepository
    {
        protected readonly IUserHelper _userHelper = userHelper
            ?? throw new ArgumentNullException(nameof(userHelper));

        public async Task<IList<NotificationEntity>> GetActualNotificationByCurrentUserAsync()
        {
            return await GetRecordsByQueryAsync(x => x.IsReady == false 
                && x.RecipientId == userHelper.GetCurrentUserId());
        }
    }
}
