using ElectronicLearningSystemWebApi.Models.UserModel.Response;
using Microsoft.AspNetCore.Mvc;
using ElectronicLearningSystemWebApi.Repositories.User;
using ElectronicLearningSystemWebApi.Helpers;
using ElectronicLearningSystemWebApi.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;

namespace ElectronicLearningSystemWebApi.Controllers
{
    /// <summary>
    /// Контролер для работы с авторизацией пользователя в системе..
    /// </summary>
    /// <param name="jwtTokenHelper">Хелпер для работы с токенами.</param>
    /// <param name="userRepository">Репозиторий для работы с пользователями системы.</param>
    [Route("auth")]
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
        /// <param name="userLoginRequest">Данные пользователя для входа.</param>
        /// <returns>Токен доступа и рефреш-токен.</returns>
        /// <response code="200">Успешная авторизация. </response>
        /// <response code="400">Некорректный запрос. </response>
        /// <response code="401">Неверный логин или пароль. </response>
        /// <response code="403">Аккаунт заблокирован. </response>
        /// <response code="500">Ошибка сервера. </response>
        [HttpPost("login")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Login([FromBody] UserLoginDTO userLoginRequest)
        {
            if (userLoginRequest is null)
            {
                return BadRequest(new
                {
                    ErrorCode = "INVALID_REQUEST",
                    Message = "Invalid request data"
                });
            }

            try
            {
                var user = await _userRepository.GetUserByLoginAsync(userLoginRequest.Login);
                if (user == null || !_userHelper.VerificationPassword(user, userLoginRequest.Password))
                {
                    return Unauthorized(new
                    {
                        ErrorCode = "ACCOUNT_UNAUTHORIZED",
                        Message = "The user entered the wrong username or password"
                    });
                }

                if (user.IsLocked)
                {
                    return StatusCode(403, new
                    {
                        ErrorCode = "ACCOUNT_BLOCKED",
                        Message = "The user's account has been deactivated"
                    });
                }

                var token = await _tokenHelper.GenerateTokenForUser(user);

                return Ok(new
                {
                    token.AccessToken,
                    token.RefreshToken
                });
            }
            catch (Exception ex)
            {
                _logger.LogError((int)EventLoggerEnum.DataBaseException, ex, "Error during user login");

                return StatusCode(500, new
                {
                    ErrorCode = "SERVER_ERROR",
                    Message = "An unexpected error occurred. Please try again later."
                });
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

        [HttpPost("recoverypassword")]
        public async Task<IActionResult> RecoveryPassword([FromBody] RecoveryPasswordDTO login) 
        {
            if (login == null)
            {
                return BadRequest(new
                {
                    ErrorCode = "INVALID_REQUEST",
                    Message = "Invalid request data"
                });
            }

            var user = await _userRepository.GetUserByLoginAsync(login.Login);
            if (user is null)
            {
                return StatusCode(404, new
                {
                    ErrorCode = "NOT_FOUND",
                    Message = "User was not found"
                });
            }

            return Ok();
        }
    }
}
