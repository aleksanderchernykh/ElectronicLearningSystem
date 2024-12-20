using ElectronicLearningSystemWebApi.Enums;
using ElectronicLearningSystemWebApi.Repositories.Notification;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ElectronicLearningSystemWebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("notification")]
    public class NotificationController(INotificationRepository notificationRepository,
        ILogger<NotificationController> logger)
        : ControllerBase
    {
        private readonly INotificationRepository _notificationRepository =
            notificationRepository ?? throw new ArgumentNullException(nameof(logger));

        private readonly ILogger<NotificationController> _logger =
            logger ?? throw new ArgumentNullException(nameof(logger));

        /// <summary>
        /// Получение ролей пользователя.
        /// </summary>
        /// <returns></returns>
        [HttpGet("getnotifications")]
        public async Task<IActionResult> GetNotifications()
        {
            try
            {
                return Ok(await _notificationRepository.GetActualNotificationByCurrentUserAsync());
            }
            catch (Exception ex)
            {
                _logger.LogError(new EventId((int)EventLoggerEnum.DataBaseException), message: ex.ToString());
                return BadRequest(ex.Message);
            }
        }
    }
}