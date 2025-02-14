using ElectronicLearningSystemWebApi.Models.NotificationModel.DTO;
using ElectronicLearningSystemWebApi.Models.NotificationModel.Response;

namespace ElectronicLearningSystemWebApi.Helpers.Services.NotificationService
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
