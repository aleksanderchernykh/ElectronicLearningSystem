using ElectronicLearningSystem.Application.Models.NotificationModel.DTO;
using ElectronicLearningSystem.Application.Models.NotificationModel.Response;

namespace ElectronicLearningSystem.Application.Services.NotificationService
{
    /// <summary>
    /// Интерфейс сервиса для работы с уведомлениями.
    /// </summary>
    public interface INotificationService
    {
        /// <summary>
        /// Получение актуальных уведомлений по текущему пользователю.
        /// </summary>
        Task<IList<NotificationResponse>> GetActualNotificationByCurrentUserAsync();

        /// <summary>
        /// Создание уведомления.
        /// </summary>
        /// <param name="createNotificationDTO">Данные для создания уведомления.</param>
        Task CreateNotificationAsync(CreateNotificationDTO createNotificationDTO);
    }
}
