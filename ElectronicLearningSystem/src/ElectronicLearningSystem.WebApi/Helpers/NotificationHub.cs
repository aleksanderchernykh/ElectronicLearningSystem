using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace ElectronicLearningSystem.WebApi.Helpers
{
    [Authorize]
    public class NotificationHub(ILogger<NotificationHub> logger) : Hub
    {
        protected readonly ILogger<NotificationHub> _logger = logger
            ?? throw new ArgumentNullException(nameof(logger));

        public async override Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
            await Clients.Caller.SendAsync("Message", "Connected successfully!");
        }

        /// <summary>
        /// СОздание оповещения для пользователя. 
        /// </summary>
        /// <param name="userId">Идентификатор пользователя. </param>
        public async Task CreatedNotification(Guid userId)
        {
            try
            {
                await Clients.All.SendAsync("CreatedNotificationFromUser", userId);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred in CreatedNotification: {ex.Message}");
            }
        }
    }
}