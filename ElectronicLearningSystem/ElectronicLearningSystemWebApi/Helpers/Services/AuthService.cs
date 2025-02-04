using ElectronicLearningSystemWebApi.Models.UserModel.DTO;
using ElectronicLearningSystemWebApi.Models.UserModel.Entity;
using ElectronicLearningSystemWebApi.Models.UserModel.Response;
using ElectronicLearningSystemWebApi.Repositories.User;

namespace ElectronicLearningSystemWebApi.Helpers.Services
{
    /// <summary>
    /// Хелпер для работы с аутентификацией пользователя.
    /// </summary>
    /// <param name="userRepository">Репозиторий пользователя. </param>
    /// <param name="userHelper">Хелпер для работы с пользователями. </param>
    /// <param name="tokenHelper">Хелпер для работы с токенами. </param>
    /// <param name="redisHelper">Хелпер для работы с Redis. </param>
    /// <param name="emailSendingHelper">Хелпер для работы с Email. </param>
    public class AuthService(IUserRepository userRepository,
        UserService userHelper,
        JwtTokenHelper tokenHelper,
        EmailSendingHelper emailSendingHelper,
        RedisHelper redisHelper)
    {
        /// <summary>
        /// Хелпер для работы с Redis.
        /// </summary>
        protected readonly RedisHelper _redisHelper = redisHelper
            ?? throw new ArgumentNullException(nameof(redisHelper));

        /// <summary>
        /// Репозиторий пользователя. 
        /// </summary>
        protected readonly IUserRepository _userRepository = userRepository
            ?? throw new ArgumentNullException(nameof(_userRepository));

        /// <summary>
        /// Хелпер для работы с пользователями. 
        /// </summary>
        protected readonly UserService _userHelper = userHelper
            ?? throw new ArgumentNullException(nameof(_userHelper));

        /// <summary>
        /// Хелпер для работы с токенами. 
        /// </summary>
        protected readonly JwtTokenHelper _tokenHelper = tokenHelper
            ?? throw new ArgumentNullException(nameof(_tokenHelper));

        /// <summary>
        /// Хелпер для работы с сообщениями.
        /// </summary>
        protected readonly EmailSendingHelper _emailSendingHelper = emailSendingHelper
            ?? throw new ArgumentNullException(nameof(_emailSendingHelper));

        /// <summary>
        /// Аутентификация пользователя.
        /// </summary>
        /// <param name="userLoginRequest">Данные пользователя для аутентификации. </param>
        /// <returns>Токены пользователя.</returns>
        /// <exception cref="UnauthorizedAccessException">Ошибка проверки пользователя и данных. </exception>
        /// <exception cref="InvalidOperationException">Ошибка проверки блокировки пользователя. </exception>
        public async Task<AccessTokenResponse> LoginAsync(UserLoginDTO userLoginRequest)
        {
            var user = await _userRepository.GetUserByLoginAsync(userLoginRequest.Login) ??
                throw new UnauthorizedAccessException("The user entered the wrong username");

            if (!VerificationPassword(user, userLoginRequest.Password))
                throw new UnauthorizedAccessException("The user entered the wrong username or password");

            if (user.IsLocked)
                throw new InvalidOperationException("The user's account has been deactivated");

            var (AccessToken, RefreshToken) = await _tokenHelper.GenerateTokenForUser(user);

            return new AccessTokenResponse
            {
                AccessToken = AccessToken,
                RefreshToken = RefreshToken
            };
        }

        /// <summary>
        /// Выход пользователя из системы.
        /// </summary>
        /// <param name="logoutDTO">Данные пользователя для выхода из системы. </param>
        /// <exception cref="KeyNotFoundException">Ошибка проверки пользователя. </exception>
        public async Task LogoutAsync(LogoutDTO logoutDTO)
        {
            var user = await _userRepository.GetUserByLoginAsync(logoutDTO.Login)
                ?? throw new KeyNotFoundException("The user entered the wrong username");

            await LogoutUserAsync(user);
        }

        /// <summary>
        /// Генерация новых токенов пользователя.
        /// </summary>
        /// <param name="refreshTokenRequest">Данные старых токенов для проверки.</param>
        /// <returns>Токены пользователя.</returns>
        /// <exception cref="UnauthorizedAccessException">Ошибка проверки пользователя.</exception>
        public async Task<AccessTokenResponse> RefreshTokenAsync(RefreshTokenDTO refreshTokenRequest)
        {
            var principal = _tokenHelper.GetPrincipalFromExpiredToken(refreshTokenRequest.AccessToken)
                ?? throw new UnauthorizedAccessException("Invalid access token");

            var user = await _userRepository.GetUserByLoginAsync(principal?.Identity?.Name)
                ?? throw new UnauthorizedAccessException("The user was not found using the transferred token");

            if (user.RefreshToken != refreshTokenRequest.RefreshToken)
                throw new UnauthorizedAccessException("Invalid user's refresh token");

            var (AccessToken, RefreshToken) = await _tokenHelper.GenerateTokenForUser(user);

            return new AccessTokenResponse
            {
                AccessToken = AccessToken,
                RefreshToken = RefreshToken
            };
        }

        /// <summary>
        /// Восстановление пароля пользователя.
        /// </summary>
        /// <param name="recoveryPasswordDTO">Данные пользователя для восстановления пароля. </param>
        /// <exception cref="KeyNotFoundException">Ошибка поиска пользователя. </exception>
        public async Task RecoveryPasswordAsync(RecoveryPasswordDTO recoveryPasswordDTO)
        {
            var user = await _userRepository.GetUserByLoginAsync(recoveryPasswordDTO.Login)
                ?? throw new KeyNotFoundException("The user entered the wrong username");

            var token = Guid.NewGuid().ToString();

            await _redisHelper.RecoveryPasswordAsync(token, user.Login, TimeSpan.FromHours(1));
            await _emailSendingHelper.SendRecoveryPasswordAsync(user, token);
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
            ArgumentNullException.ThrowIfNull(user);
            ArgumentException.ThrowIfNullOrWhiteSpace(password);

            return PasswordHelper.VerifyPassword(user.Password, password);
        }
    }
}