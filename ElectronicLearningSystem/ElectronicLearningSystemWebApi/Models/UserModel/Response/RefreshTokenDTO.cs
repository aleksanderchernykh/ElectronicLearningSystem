using System.ComponentModel.DataAnnotations;

namespace ElectronicLearningSystemWebApi.Models.UserModel.Response
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
