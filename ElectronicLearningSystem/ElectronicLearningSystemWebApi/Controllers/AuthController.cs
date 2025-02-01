using ElectronicLearningSystemWebApi.Models.UserModel.Response;
using Microsoft.AspNetCore.Mvc;
using ElectronicLearningSystemWebApi.Helpers;
using ElectronicLearningSystemWebApi.Enums;
using ElectronicLearningSystemWebApi.Models.ErrorModel;

namespace ElectronicLearningSystemWebApi.Controllers
{
    /// <summary>
    /// Контролер для работы с авторизацией пользователя в системе.
    /// </summary>
    /// <param name="authHelper">Хелпер для работы с авторизацией пользователя в системе. </param>
    /// <param name="logger">Логгер. </param>
    [Route("auth")]
    [ApiController]
    public class AuthController(
        AuthHelper authHelper,
        ILogger<AuthController> logger) : ControllerBase
    {
        /// <summary>
        /// Хелпер для работы с авторизацией пользователя в системе. 
        /// </summary>
        private readonly AuthHelper _authHelper =
            authHelper ?? throw new ArgumentNullException(nameof(authHelper));

        /// <summary>
        /// Логгер. 
        /// </summary>
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
        [ProducesResponseType(typeof(AccessTokenResponse), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        [ProducesResponseType(typeof(ErrorResponse), 401)]
        [ProducesResponseType(typeof(ErrorResponse), 403)]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        public async Task<IActionResult> Login([FromBody] UserLoginDTO userLoginRequest)
        {
            try
            {
                var token = await _authHelper.LoginAsync(userLoginRequest);

                return Ok(token);
            }
            catch (BadHttpRequestException ex)
            {
                return BadRequest(new ErrorResponse
                {
                    ErrorCode = "INVALID_REQUEST",
                    ErrorMessage = ex.Message
                });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new ErrorResponse
                {
                    ErrorCode = "ACCOUNT_UNAUTHORIZED",
                    ErrorMessage = ex.Message
                });
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(403, new ErrorResponse
                {
                    ErrorCode = "ACCOUNT_BLOCKED",
                    ErrorMessage = ex.Message
                });
            }
            catch (Exception ex)
            {
                _logger.LogError((int)EventLoggerEnum.DataBaseException, ex.ToString());

                return StatusCode(500, new ErrorResponse
                {
                    ErrorCode = "SERVER_ERROR",
                    ErrorMessage = "An unexpected error occurred. Please try again later."
                });
            }
        }

        /// <summary>
        /// Выход пользователя из системы.
        /// </summary>
        /// <param name="logoutResponse">Данные пользователя для выхода из системы. </param>
        /// <response code="200">Успешный выход из системы. </response>
        /// <response code="401">Неверно переданы данные пользователя. </response>
        /// <response code="500">Ошибка сервера. </response>
        [HttpPost("logout")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ErrorResponse), 401)]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        public async Task<IActionResult> Logout([FromBody] LogoutDTO logoutResponse)
        {
            try
            {
                await _authHelper.LogoutAsync(logoutResponse);

                return Ok();
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new ErrorResponse
                {
                    ErrorCode = "ACCOUNT_UNAUTHORIZED",
                    ErrorMessage = ex.Message
                });
            }
            catch (Exception ex)
            {
                _logger.LogError((int)EventLoggerEnum.DataBaseException, ex.ToString());

                return StatusCode(500, new ErrorResponse
                {
                    ErrorCode = "SERVER_ERROR",
                    ErrorMessage = "An unexpected error occurred. Please try again later."
                });
            }
        }

        /// <summary>
        /// Обновление токена пользователя.
        /// </summary>
        /// <param name="refreshTokenRequest">Запрос на обновление токена.</param>
        /// <returns>Новые токены доступа.</returns>
        /// <response code="200">Успешное обновление токенов доступа. </response>
        /// <response code="401">Неверно переданы данные пользователя. </response>
        /// <response code="500">Ошибка сервера. </response>
        [HttpPost("refreshtoken")]
        [ProducesResponseType(typeof(AccessTokenResponse), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 401)]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenDTO refreshTokenRequest)
        {
            try
            {
                var token = await _authHelper.RefreshTokenAsync(refreshTokenRequest);

                return Ok(token);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new ErrorResponse
                {
                    ErrorCode = "ACCOUNT_UNAUTHORIZED",
                    ErrorMessage = ex.Message
                });
            }
            catch (Exception ex)
            {
                _logger.LogError((int)EventLoggerEnum.DataBaseException, ex.ToString());
                return StatusCode(500, new ErrorResponse
                {
                    ErrorCode = "SERVER_ERROR",
                    ErrorMessage = "An unexpected error occurred. Please try again later."
                });
            }
        }

        /// <summary>
        /// Восстановление пароля для пользователя.
        /// </summary>
        /// <param name="recoveryPasswordDTO">данные пользователя для восстановления пароля. </param>
        /// <response code="200">Успешный запрос на восстановление пололя пользователя. </response>
        /// <response code="500">Ошибка сервера. </response>
        [HttpPost("recoverypassword")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        public async Task<IActionResult> RecoveryPassword([FromBody] RecoveryPasswordDTO recoveryPasswordDTO) 
        {
            try
            {
                await _authHelper.RecoveryPasswordAsync(recoveryPasswordDTO);

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError((int)EventLoggerEnum.DataBaseException, ex.ToString());

                return StatusCode(500, new ErrorResponse
                {
                    ErrorCode = "SERVER_ERROR",
                    ErrorMessage = "An unexpected error occurred. Please try again later."
                });
            }
        }
    }
}
