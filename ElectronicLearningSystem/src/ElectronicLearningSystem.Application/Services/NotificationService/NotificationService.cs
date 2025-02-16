using AutoMapper;
using ElectronicLearningSystem.Application.Models.NotificationModel.DTO;
using ElectronicLearningSystem.Application.Models.NotificationModel.Response;
using ElectronicLearningSystem.Infrastructure.Models.NotificationModel;
using ElectronicLearningSystem.Infrastructure.Repositories.Notification;

namespace ElectronicLearningSystem.Application.Services.NotificationService
{
    /// <summary>
    /// Хелпер для работы с уведомлениями.
    /// </summary>
    /// <param name="userHelper">Хелпер для работы с пользователем. </param>
    /// <param name="notificationRepository">Репозиторий для работы с уведомлениями. </param>
    /// <param name="mapper">Маппер. </param>
    public class NotificationService(IUserService userHelper,
        INotificationRepository notificationRepository,
        IMapper mapper) : INotificationService
    {
        /// <summary>
        /// Хелпер для работы с пользователем.
        /// </summary>
        protected readonly IUserService _userHelper = userHelper
            ?? throw new ArgumentNullException(nameof(userHelper));

        /// <summary>
        /// Репозиторий для работы с уведомлениями.
        /// </summary>
        protected readonly INotificationRepository _notificationRepository =
            notificationRepository ?? throw new ArgumentNullException(nameof(notificationRepository));

        /// <summary>
        /// Маппер. 
        /// </summary>
        protected readonly IMapper _mapper = mapper
            ?? throw new ArgumentNullException(nameof(mapper));

        /// <summary>
        /// Получение актуальных уведомлений по текущему пользователю.
        /// </summary>
        public async Task<IList<NotificationResponse>> GetActualNotificationByCurrentUserAsync()
        {
            var notifications = await _notificationRepository.GetActualNotificationByCurrentUserAsync();
            return _mapper.Map<IList<NotificationResponse>>(notifications);
        }

        /// <summary>
        /// Создание уведомления.
        /// </summary>
        /// <param name="createNotificationDTO">Данные для создания уведомления. </param>
        public async Task CreateNotificationAsync(CreateNotificationDTO createNotificationDTO)
        {
            var comment = _mapper.Map<NotificationEntity>(createNotificationDTO);
            await _notificationRepository.AddRecordAsync(comment);
        }
    }
}
