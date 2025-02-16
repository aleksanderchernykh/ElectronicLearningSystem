using System.ComponentModel.DataAnnotations;

namespace ElectronicLearningSystem.Application.Models.UserModel.DTO
{
    /// <summary>
    /// Запрос на обновление токена пользователя.
    /// </summary>
    public class RefreshTokenDTO
    {
        /// <summary>
        /// Токен доступа.
        /// </summary>
        public required string AccessToken { get; set; }

        /// <summary>
        /// Токен для обновления.
        /// </summary>
        public required string RefreshToken { get; set; }
    }
}
