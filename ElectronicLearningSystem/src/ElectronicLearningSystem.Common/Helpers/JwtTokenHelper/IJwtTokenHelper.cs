using ElectronicLearningSystem.Infrastructure.Models.UserModel;
using System.Security.Claims;

namespace ElectronicLearningSystem.Common.Helpers.JwtTokenHelper
{
    /// <summary>
    /// Хелпер для генерации и проверки токена пользователя.
    /// </summary>
    public interface IJwtTokenHelper
    {
        /// <summary>
        /// Проверка токена пользователя, совершившего запрос.
        /// </summary>
        /// <param name="token">Токен. </param>
        /// <returns>Пользователь. </returns>
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);

        /// <summary>
        /// Генерация токена для пользователя.
        /// </summary>
        /// <param name="user">Пользователь. </param>
        /// <returns>Токен доступа и обновления. </returns>
        Task<(string AccessToken, string RefreshToken)> GenerateTokenForUser(UserEntity user);
    }
}
