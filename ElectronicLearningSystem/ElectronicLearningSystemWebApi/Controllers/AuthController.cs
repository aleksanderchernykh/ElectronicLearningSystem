using ElectronicLearningSystemKafka.Core.Producer;
using ElectronicLearningSystemWebApi.Helpers.Jwt;
using ElectronicLearningSystemWebApi.Models.EmailModel.Response;
using ElectronicLearningSystemWebApi.Models.UserModel.Response;
using ElectronicLearningSystemWebApi.Repositories;
using Microsoft.AspNetCore.Mvc;
using ElectronicLearningSystemKafka.Common.Enums;
using Confluent.Kafka;
using ElectronicLearningSystemKafka.Common.Models;

namespace ElectronicLearningSystemWebApi.Controllers
{
    /// <summary>
    /// Контролер для работы с авторизацией пользователя в системе..
    /// </summary>
    /// <param name="tokenHelper">Хелпер для работы с токенами.</param>
    /// <param name="configuration">Конфигурация.</param>
    /// <param name="userRepository">Репозиторий для работы с пользователями системы.</param>
    [ApiController]
    [Route("auth")]
    public class AuthController(TokenHelper tokenHelper,
        IConfiguration configuration,
        UserRepository userRepository,
        ILogger<AuthController> logger,
        Producer producer) : ControllerBase
    {
        private ILogger<AuthController> _logger = logger;

        private readonly Producer _producer = producer;

        /// <summary>
        /// Хелпер для работы с токенами.
        /// </summary>
        private readonly TokenHelper _tokenHelper = tokenHelper;

        /// <summary>
        /// Конфигурация.
        /// </summary>
        private readonly IConfiguration _configuration = configuration;

        /// <summary>
        /// Репозиторий для работы с пользователями системы.
        /// </summary>
        private readonly UserRepository _userRepository = userRepository;

        /// <summary>
        /// Авторизация пользователя в системе.
        /// </summary>
        /// <param name="userLoginResponse">Запрос на авторизацию.</param>
        /// <returns>Токен для дальнейшего доступа пользователя в системе.</returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginRequest userLoginResponse)
        {
            try
            {
                var user = await _userRepository.GetUserByLoginAsync(userLoginResponse.Login);
                if (user == null || !_userRepository.VerificationPassword(user, userLoginResponse.Password))
                {
                    return Unauthorized("Некорректно передан логин или пароль.");
                }

                var token = _tokenHelper.GenerateTokenForUser(user);

                return Ok(new 
                { 
                    AccessToken = token.Item1, 
                    RefreshToken = token.Item2 
                });
            }
            catch
            {
                return Unauthorized();
            }
        }

        /// <summary>
        /// Выход пользователя из системы.
        /// </summary>
        /// <param name="userLoginResponse"></param>
        /// <returns></returns>
        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] LogoutRequest logoutResponse)
        {
            var user = await _userRepository.GetUserByLoginAsync(logoutResponse.Login);
            if (user == null)
            {
                return Unauthorized("Некорректно передан логин или пароль.");
            }

            _userRepository.LogoutUser(user);
            return Ok();
        }

        /// <summary>
        /// Обновление токена пользователя.
        /// </summary>
        /// <param name="refreshTokenRequest">Запрос на обновление токена.</param>
        /// <returns>Новые токены доступа.</returns>
        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest refreshTokenRequest)
        {
            try
            {
                var principal = tokenHelper.GetPrincipalFromExpiredToken(refreshTokenRequest.AccessToken);
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

                var token = _tokenHelper.GenerateTokenForUser(user);

                return Ok(new
                {
                    AccessToken = token.Item1,
                    RefreshToken = token.Item2
                });
            }
            catch
            {
                return Unauthorized();
            }
        }

        /// <summary>
        /// Отправка сообщения для проверки kafka.
        /// </summary>
        [HttpPost("sendemail")]
        public async Task<IActionResult> SendEmail([FromBody] EmailSendingResponse emailResponse)
        {
            try
            {
                var email = new Email 
                {
                    Text = emailResponse.Text,
                    Recipients = emailResponse.Recipients,
                    Files = emailResponse.Files,
                    Subject = emailResponse.Subject,
                };

                await _producer.SendMessage(TopicEnum.EmailSending, new Message<string, Email> { Key = Guid.NewGuid().ToString(), Value = email });

                return Ok();
            }
            catch
            {
                return Unauthorized();
            }
        }
    }
}
