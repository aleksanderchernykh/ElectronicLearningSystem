using ElectronicLearningSystemWebApi.Enums;
using ElectronicLearningSystemWebApi.Helpers;
using ElectronicLearningSystemWebApi.Models.CommentModel.DTO;
using ElectronicLearningSystemWebApi.Models.NotificationModel.DTO;
using ElectronicLearningSystemWebApi.Repositories.Notification;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ElectronicLearningSystemWebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("notification")]
    public class NotificationController(INotificationRepository notificationRepository,
        ILogger<NotificationController> logger,
        NotificationHelper notificationHelper): ControllerBase
    {
        private readonly INotificationRepository _notificationRepository =
            notificationRepository ?? throw new ArgumentNullException(nameof(logger));

        private readonly ILogger<NotificationController> _logger =
            logger ?? throw new ArgumentNullException(nameof(logger));

        private readonly NotificationHelper _notificationHelper = 
            notificationHelper ?? throw new ArgumentNullException(nameof(notificationHelper));

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

        [HttpPost("createnotification")]
        public async Task<IActionResult> CreateCommentByTask([FromBody] CreateNotificationDTO createNotificationDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                var comment = _notificationHelper.GetNotificationByDTO(createNotificationDTO);
                await _notificationRepository.AddRecordAsync(comment);

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(new EventId((int)EventLoggerEnum.DataBaseException),
                    message: ex.ToString());
                return BadRequest();
            }
        }
    }
}