using ElectronicLearningSystemWebApi.Models.UserModel.DTO;
using ElectronicLearningSystemWebApi.Models.UserModel.Entity;
using ElectronicLearningSystemWebApi.Models;
using ElectronicLearningSystemWebApi.Models.UserModel.Response;

namespace ElectronicLearningSystemWebApi.Services.AuthService
{
    /// <summary>
    /// Интерфейс сервиса аутентификации.
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// Аутентификация пользователя.
        /// </summary>
        /// <param name="userLoginRequest">Данные пользователя для аутентификации.</param>
        /// <returns>Токены пользователя.</returns>
        Task<AccessTokenResponse> LoginAsync(UserLoginDTO userLoginRequest);

        /// <summary>
        /// Выход пользователя из системы.
        /// </summary>
        /// <param name="logoutDTO">Данные пользователя для выхода из системы.</param>
        Task LogoutAsync(LogoutDTO logoutDTO);

        /// <summary>
        /// Генерация новых токенов пользователя.
        /// </summary>
        /// <param name="refreshTokenRequest">Данные старых токенов для проверки.</param>
        /// <returns>Токены пользователя.</returns>
        Task<AccessTokenResponse> RefreshTokenAsync(RefreshTokenDTO refreshTokenRequest);

        /// <summary>
        /// Восстановление пароля пользователя.
        /// </summary>
        /// <param name="recoveryPasswordDTO">Данные пользователя для восстановления пароля.</param>
        Task RecoveryPasswordAsync(RecoveryPasswordDTO recoveryPasswordDTO);

        /// <summary>
        /// Получение пользователя по токену восстановления пароля.
        /// </summary>
        /// <param name="id">Токен восстановления пароля.</param>
        /// <returns>Анонимный пользователь.</returns>
        Task<BaseResponse> GetUserIdByRecoveryPasswordTokenAsync(Guid id);

        /// <summary>
        /// Выход пользователя из системы.
        /// </summary>
        /// <param name="user">Пользователь.</param>
        Task LogoutUserAsync(UserEntity user);

        /// <summary>
        /// Проверка пароля пользователя.
        /// </summary>
        /// <param name="user">Пользователь.</param>
        /// <param name="password">Пароль.</param>
        /// <returns>Логическое значение равенства паролей.</returns>
        bool VerificationPassword(UserEntity user, string password);
    }
}
