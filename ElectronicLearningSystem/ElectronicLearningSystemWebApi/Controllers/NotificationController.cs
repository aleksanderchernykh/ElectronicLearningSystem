using ElectronicLearningSystemWebApi.Attributes;
using ElectronicLearningSystemWebApi.Enums;
using ElectronicLearningSystemWebApi.Helpers.Controller;
using ElectronicLearningSystemWebApi.Models.CommentModel.DTO;
using ElectronicLearningSystemWebApi.Models.ErrorModel;
using ElectronicLearningSystemWebApi.Models.NotificationModel.DTO;
using ElectronicLearningSystemWebApi.Models.NotificationModel.Response;
using ElectronicLearningSystemWebApi.Repositories.Notification;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ElectronicLearningSystemWebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("notification")]
    public class NotificationController(NotificationHelper notificationHelper)
        : ControllerBase
    {
        /// <summary>
        /// Хелпер для работы с уведомлениями.
        /// </summary>
        private readonly NotificationHelper _notificationHelper = 
            notificationHelper ?? throw new ArgumentNullException(nameof(notificationHelper));

        /// <summary>
        /// Получение актуальных уведомлений по текущему пользователю.
        /// </summary>
        /// <response code="200">Успешный возврат актуальных уведомлений. </response>
        /// <response code="500">Ошибка сервера. </response>
        [Produces("application/json")]
        [ProducesResponseType(typeof(NotificationResponse), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        [HttpGet("get")]
        public async Task<IActionResult> GetActualNotificationByCurrentUser()
        {
            var notifications = await _notificationHelper.GetActualNotificationByCurrentUserAsync();
            return Ok(notifications);
        }

        /// <summary>
        /// Создание уведомления.
        /// </summary>
        /// <param name="createNotificationDTO">Данные для создания уведомления. </param>
        /// <response code="201">Успешное создание уведомления. </response>
        /// <response code="500">Ошибка сервера. </response>
        [ValidateModel]
        [Produces("application/json")]
        [ProducesResponseType(201)]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        [HttpPost("create")]
        public async Task<IActionResult> CreateCommentByTask([FromBody] CreateNotificationDTO createNotificationDTO)
        {
            await _notificationHelper.CreateCommentByTaskAsync(createNotificationDTO);
            return Created();
        }
    }
}