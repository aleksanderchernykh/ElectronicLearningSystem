using ElectronicLearningSystemWebApi.Models.NotificationModel;
using ElectronicLearningSystemWebApi.Models.UserModel;
using ElectronicLearningSystemWebApi.Repositories.Base;

namespace ElectronicLearningSystemWebApi.Repositories.Notification
{
    public interface INotificationRepository : IRepository<NotificationEntity>
    {
        Task<IList<NotificationEntity>> GetActualNotificationByCurrentUserAsync();
    }
}
