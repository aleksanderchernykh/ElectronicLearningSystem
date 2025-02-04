using AutoMapper;
using ElectronicLearningSystemWebApi.Models.NotificationModel.DTO;
using ElectronicLearningSystemWebApi.Models.NotificationModel.Entity;
using ElectronicLearningSystemWebApi.Models.NotificationModel.Response;
using ElectronicLearningSystemWebApi.Repositories.Notification;

namespace ElectronicLearningSystemWebApi.Helpers.Controller
{
    /// <summary>
    /// Хелпер для работы с уведомлениями.
    /// </summary>
    /// <param name="userHelper">Хелпер для работы с пользователем. </param>
    /// <param name="notificationRepository">Репозиторий для работы с уведомлениями. </param>
    /// <param name="mapper">Маппер. </param>
    public class NotificationHelper(UserHelper userHelper,
        INotificationRepository notificationRepository,
        IMapper mapper)
    {
        /// <summary>
        /// Хелпер для работы с пользователем.
        /// </summary>
        protected readonly UserHelper _userHelper = userHelper
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
        /// Создание комментария по задаче.
        /// </summary>
        /// <param name="createNotificationDTO">Данные для создания комментария. </param>
        public async Task CreateCommentByTaskAsync(CreateNotificationDTO createNotificationDTO)
        {
            var comment = _mapper.Map<NotificationEntity>(createNotificationDTO);
            await _notificationRepository.AddRecordAsync(comment);
        }
    }
}
