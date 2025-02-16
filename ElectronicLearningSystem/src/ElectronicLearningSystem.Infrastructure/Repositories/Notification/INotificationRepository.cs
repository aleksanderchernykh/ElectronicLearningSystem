using ElectronicLearningSystem.Infrastructure.Models.NotificationModel;
using ElectronicLearningSystem.Infrastructure.Repositories.Base;

namespace ElectronicLearningSystem.Infrastructure.Repositories.Notification
{
    public interface INotificationRepository : IRepository<NotificationEntity>
    {
        Task<IList<NotificationEntity>> GetActualNotificationByCurrentUserAsync();
    }
}
