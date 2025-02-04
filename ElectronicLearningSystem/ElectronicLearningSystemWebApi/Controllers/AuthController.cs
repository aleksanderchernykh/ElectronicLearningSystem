using ElectronicLearningSystemWebApi.Models.UserModel.Response;
using Microsoft.AspNetCore.Mvc;
using ElectronicLearningSystemWebApi.Models.ErrorModel;
using ElectronicLearningSystemWebApi.Attributes;
using ElectronicLearningSystemWebApi.Helpers.Controller;

namespace ElectronicLearningSystemWebApi.Controllers
{
    /// <summary>
    /// Контролер для работы с авторизацией пользователя в системе.
    /// </summary>
    /// <param name="authHelper">Хелпер для работы с авторизацией пользователя в системе. </param>
    [Route("auth")]
    [ApiController]
    public class AuthController(AuthHelper authHelper) 
        : ControllerBase
    {
        /// <summary>
        /// Хелпер для работы с авторизацией пользователя в системе. 
        /// </summary>
        private readonly AuthHelper _authHelper = authHelper 
            ?? throw new ArgumentNullException(nameof(authHelper));

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
        [Produces("application/json")]
        [ValidateModel]
        [ProducesResponseType(typeof(AccessTokenResponse), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 401)]
        [ProducesResponseType(typeof(ErrorResponse), 403)]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        public async Task<IActionResult> Login([FromBody] UserLoginDTO userLoginRequest)
        {
            var token = await _authHelper.LoginAsync(userLoginRequest);
            return Ok(token);
        }

        /// <summary>
        /// Выход пользователя из системы.
        /// </summary>
        /// <param name="logoutDTO">Данные пользователя для выхода из системы. </param>
        /// <response code="200">Успешный выход из системы. </response>
        /// <response code="404">Ошибка поиска пользователя. </response>
        /// <response code="500">Ошибка сервера. </response>
        [HttpPost("logout")]
        [Produces("application/json")]
        [ValidateModel]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ErrorResponse), 404)]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        public async Task<IActionResult> Logout([FromBody] LogoutDTO logoutDTO)
        {
            await _authHelper.LogoutAsync(logoutDTO);
            return Ok();
        }

        /// <summary>
        /// Обновление токена пользователя.
        /// </summary>
        /// <param name="refreshTokenDTO">Запрос на обновление токена.</param>
        /// <returns>Новые токены доступа.</returns>
        /// <response code="200">Успешное обновление токенов доступа. </response>
        /// <response code="401">Неверно переданы данные пользователя. </response>
        /// <response code="500">Ошибка сервера. </response>
        [HttpPost("refreshtoken")]
        [Produces("application/json")]
        [ValidateModel]
        [ProducesResponseType(typeof(AccessTokenResponse), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 401)]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenDTO refreshTokenDTO)
        {
            var token = await _authHelper.RefreshTokenAsync(refreshTokenDTO);
            return Ok(token);
        }

        /// <summary>
        /// Восстановление пароля для пользователя.
        /// </summary>
        /// <param name="recoveryPasswordDTO">данные пользователя для восстановления пароля. </param>
        /// <response code="200">Успешный запрос на восстановление пололя пользователя. </response>
        /// <response code="404">Ошибка поиска пользователя. </response>
        /// <response code="500">Ошибка сервера. </response>
        [HttpPost("recoverypassword")]
        [Produces("application/json")]
        [ValidateModel]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ErrorResponse), 404)]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        public async Task<IActionResult> RecoveryPassword([FromBody] RecoveryPasswordDTO recoveryPasswordDTO) 
        {
            await _authHelper.RecoveryPasswordAsync(recoveryPasswordDTO);
            return Ok();
        }
    }
}
