using ElectronicLearningSystemWebApi.Helpers.Jwt;
using ElectronicLearningSystemWebApi.Models.UserModel;
using ElectronicLearningSystemWebApi.Models.UserModel.Response;
using ElectronicLearningSystemWebApi.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ElectronicLearningSystemWebApi.Controllers
{
    /// <summary>
    /// ctor.
    /// </summary>
    /// <param name="tokenHelper">������ ��� ������ � ��������.</param>
    /// <param name="configuration">������������ ���.</param>
    /// <param name="userRepository">����������� ��� ������ � �������������.</param>
    [ApiController]
    [Route("auth")]
    public class AuthController(TokenHelper tokenHelper,
        IConfiguration configuration,
        UserRepository userRepository) : ControllerBase
    {
        /// <summary>
        /// ������ ��� ������ � ��������.
        /// </summary>
        private readonly TokenHelper _tokenHelper = tokenHelper;

        /// <summary>
        /// ������������ ���.
        /// </summary>
        private readonly IConfiguration _configuration = configuration;

        /// <summary>
        /// ����������� ��� ������ � �������������.
        /// </summary>
        private readonly UserRepository _userRepository = userRepository;

        /// <summary>
        /// �������������� ������������.
        /// </summary>
        /// <param name="userLoginResponse">������ ������������ ��� ����.</param>
        /// <returns>����� �������������� � ������ �����.</returns>
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

                var token = GenerateTokenForUser(user);

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

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest refreshTokenRequest)
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

                var token = GenerateTokenForUser(user);

                // Возвращаем новые токены клиенту
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
        /// ��������� ������ ��� ������������.
        /// </summary>
        /// <param name="user">������������.</param>
        /// <returns>����� �������������� � ������ ����а.</returns>
        private Tuple<string, string> GenerateTokenForUser(User user)
        {
            var accessToken = _tokenHelper.GenerateAccessToken(user.Login);
            var refreshToken = _tokenHelper.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(Convert.ToDouble(_configuration["Jwt:RefreshTokenLifetime"]));
            _userRepository.UpdateUser(user);

            return Tuple.Create(accessToken, refreshToken);
        }
    }
}
