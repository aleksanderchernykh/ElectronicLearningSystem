using ElectronicLearningSystemWebApi.Models.UserModel.Response;
using Microsoft.AspNetCore.Mvc;
using ElectronicLearningSystemWebApi.Repositories.User;
using ElectronicLearningSystemWebApi.Helpers;
using ElectronicLearningSystemWebApi.Enums;

namespace ElectronicLearningSystemWebApi.Controllers
{
    /// <summary>
    /// Контролер для работы с авторизацией пользователя в системе..
    /// </summary>
    /// <param name="jwtTokenHelper">Хелпер для работы с токенами.</param>
    /// <param name="userRepository">Репозиторий для работы с пользователями системы.</param>
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(JwtTokenHelper jwtTokenHelper,
        UserHelper userHelper,
        IUserRepository userRepository,
        ILogger<AuthController> logger) : ControllerBase
    {
        private readonly UserHelper _userHelper = 
            userHelper ?? throw new ArgumentNullException(nameof(userHelper));

        private readonly JwtTokenHelper _tokenHelper = 
            jwtTokenHelper ?? throw new ArgumentNullException(nameof(jwtTokenHelper));

        private readonly IUserRepository _userRepository = 
            userRepository ?? throw new ArgumentNullException(nameof(userRepository));

        private readonly ILogger<AuthController> _logger = 
            logger ?? throw new ArgumentNullException(nameof(logger));

        /// <summary>
        /// Авторизация пользователя в системе.
        /// </summary>
        /// <param name="userLoginResponse">Запрос на авторизацию.</param>
        /// <returns>Токен для дальнейшего доступа пользователя в системе.</returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDTO userLoginResponse)
        {
            try
            {
                var user = await _userRepository.GetUserByLoginAsync(userLoginResponse.Login);
                if (user == null || !_userHelper.VerificationPassword(user, userLoginResponse.Password))
                {
                    return Unauthorized("Некорректно передан логин или пароль.");
                }

                var token = await _tokenHelper.GenerateTokenForUser(user);

                return Ok(token);
            }
            catch (Exception ex)
            {
                _logger.LogError((int)EventLoggerEnum.DataBaseException, ex.ToString());
                return Unauthorized();
            }
        }

        /// <summary>
        /// Выход пользователя из системы.
        /// </summary>
        /// <param name="userLoginResponse"></param>
        /// <returns></returns>
        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] LogoutDTO logoutResponse)
        {
            try
            {
                var user = await _userRepository.GetUserByLoginAsync(logoutResponse.Login);
                if (user == null)
                {
                    return Unauthorized("Некорректно передан логин или пароль.");
                }

                await _userHelper.LogoutUserAsync(user);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError((int)EventLoggerEnum.DataBaseException, ex.ToString());
                return Unauthorized();
            }
        }

        /// <summary>
        /// Обновление токена пользователя.
        /// </summary>
        /// <param name="refreshTokenRequest">Запрос на обновление токена.</param>
        /// <returns>Новые токены доступа.</returns>
        [HttpPost("refreshtoken")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenDTO refreshTokenRequest)
        {
            try
            {
                var principal = jwtTokenHelper.GetPrincipalFromExpiredToken(refreshTokenRequest.AccessToken);
                if (principal == null)
                {
                    return Unauthorized("Некорректный токен доступа.");
                }

                var user = await _userRepository.GetUserByLoginAsync(principal?.Identity?.Name);
                if (user == null)
                {
                    return Unauthorized("Не найден пользователь по переданному токену.");
                }

                if (user.RefreshToken != refreshTokenRequest.RefreshToken)
                {
                    return Unauthorized("Некорректный рефреш токен пользователя.");
                }

                var token = await _tokenHelper.GenerateTokenForUser(user);

                return Ok(token);
            }
            catch (Exception ex)
            {
                _logger.LogError((int)EventLoggerEnum.DataBaseException, ex.ToString());
                return Unauthorized();
            }
        }
    }
}
