using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace ElectronicLearningSystem.Core.Helpers.UserHelper
{
    public class UserHelper(IHttpContextAccessor httpContextAccessor) : IUserHelper
    {
        /// <summary>
        /// Контекст выполнения запроса.
        /// </summary>
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor
            ?? throw new ArgumentNullException(nameof(httpContextAccessor));

        /// <summary>
        /// Получение идентификатора текущего пользователя.
        /// </summary>
        /// <returns></returns>
        public virtual Guid GetCurrentUserId()
        {
            var userNameIdentifier = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            ArgumentException.ThrowIfNullOrWhiteSpace(userNameIdentifier, nameof(userNameIdentifier));

            if (string.IsNullOrEmpty(userNameIdentifier) || !Guid.TryParse(userNameIdentifier, out var userId))
                throw new ArgumentNullException(nameof(userId), "User ID is missing or invalid.");

            return userId;
        }
    }
}
