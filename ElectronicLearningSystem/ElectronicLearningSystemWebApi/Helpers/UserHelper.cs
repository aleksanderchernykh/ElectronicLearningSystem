using ElectronicLearningSystemWebApi.Models.UserModel;
using ElectronicLearningSystemWebApi.Repositories.User;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace ElectronicLearningSystemWebApi.Helpers
{
    public class UserHelper(IUserRepository userRepository,
        IHttpContextAccessor httpContextAccessor)
    {
        protected readonly IUserRepository _userRepository = userRepository 
            ?? throw new ArgumentNullException(nameof(userRepository));

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
            {
                throw new ArgumentNullException(nameof(userId), "User ID is missing or invalid.");
            }

            return userId;
        }

        /// <summary>
        /// Выход пользователя из системы.
        /// </summary>
        /// <param name="user">Пользователь.</param>
        public virtual async Task LogoutUserAsync(UserEntity user)
        {
            user.RefreshToken = null;
            user.RefreshTokenExpiryTime = null;
            await _userRepository.UpdateRecordAsync(user);
        }

        /// <summary>
        /// Проверка пароля пользователя.
        /// </summary>
        /// <param name="user">Пользователя.</param>
        /// <param name="password">Пароль.</param>
        /// <returns>Логическое значение равенства паролей.</returns>
        /// <exception cref="ArgumentNullException">Передано пустое значение.</exception>
        public virtual bool VerificationPassword(UserEntity user, string password)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(UserEntity), "Передано пустое значение.");
            }

            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentNullException(nameof(password), "Передано пустое значение пароля пользователя.");
            }

            return PasswordHelper.VerifyPassword(user.Password, password);
        }
    }
}
