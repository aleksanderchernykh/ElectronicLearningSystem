using ElectronicLearningSystemWebApi.Models.UserModel;
using ElectronicLearningSystemWebApi.Repositories;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ElectronicLearningSystemWebApi.Helpers.Jwt
{
    /// <summary>
    /// Хелпер для генерации и проверки токена пользователя.
    /// </summary>
    /// <param name="configuration">Конфигурация</param>
    /// <param name="userRepository"></param>
    public class TokenHelper(IConfiguration configuration,
        UserRepository userRepository)
    {
        /// <summary>
        /// Конфигурация приложения.
        /// </summary>
        private readonly IConfiguration _configuration = configuration;

        /// <summary>
        /// Репозиторий для работы с пользователем.
        /// </summary>
        private readonly UserRepository _userRepository = userRepository;

        /// <summary>
        /// Генерация токена для доступа.
        /// </summary>
        /// <param name="username">Логин пользователя.</param>
        /// <returns>Токен.</returns>
        public string GenerateAccessToken(string username)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, username) }),
                Expires = DateTime.UtcNow.AddHours(Convert.ToDouble(_configuration["Jwt:AccessTokenLifetime"])),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        /// <summary>
        /// Генерация рефреш токена.
        /// </summary>
        /// <returns></returns>
        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];

            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        /// <summary>
        /// Проверка токена пользователя, совершившего запрос.
        /// </summary>
        /// <param name="token">Токен.</param>
        /// <returns>Пользователь.</returns>
        /// <exception cref="SecurityTokenException">Ошибка проверки токена.</exception>
        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _configuration["Jwt:Issuer"],
                ValidAudience = _configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["Jwt:Key"])),
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;

            if (jwtSecurityToken == null 
                || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token");
            }

            return principal;
        }

        /// <summary>
        /// Генерация токена для пользователя.
        /// </summary>
        /// <param name="user">Пользователь.</param>
        /// <returns>Токен доступа и обновления.</returns>
        public Tuple<string, string> GenerateTokenForUser(User user)
        {
            var accessToken = GenerateAccessToken(user.Login);
            var refreshToken = GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(Convert.ToDouble(_configuration["Jwt:RefreshTokenLifetime"]));
            _userRepository.UpdateUser(user);

            return Tuple.Create(accessToken, refreshToken);
        }
    }
}