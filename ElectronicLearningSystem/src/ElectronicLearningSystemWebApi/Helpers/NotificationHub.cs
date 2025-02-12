using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace ElectronicLearningSystemWebApi.Helpers
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

        // Метод, который будет вызываться на клиенте
        public async Task CreatedNotification(string user)
        {
            try
            {
                await Clients.All.SendAsync("CreatedNotificationFromUser", user);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error occurred in CreatedNotification: {ex.Message}");
            }
        }
    }
}