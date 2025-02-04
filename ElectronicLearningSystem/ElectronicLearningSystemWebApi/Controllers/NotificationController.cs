using ElectronicLearningSystemWebApi.Attributes;
using ElectronicLearningSystemWebApi.Helpers.Services;
using ElectronicLearningSystemWebApi.Models.ErrorModel;
using ElectronicLearningSystemWebApi.Models.NotificationModel.DTO;
using ElectronicLearningSystemWebApi.Models.NotificationModel.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ElectronicLearningSystemWebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("notification")]
    public class NotificationController(NotificationService notificationService)
        : ControllerBase
    {
        /// <summary>
        /// Хелпер для работы с уведомлениями.
        /// </summary>
        private readonly NotificationService _notificationService = 
            notificationService ?? throw new ArgumentNullException(nameof(notificationService));

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
            var notifications = await _notificationService.GetActualNotificationByCurrentUserAsync();
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
        public async Task<IActionResult> CreateNotificationAsync([FromBody] CreateNotificationDTO createNotificationDTO)
        {
            await _notificationService.CreateNotificationAsync(createNotificationDTO);
            return Created();
        }
    }
}