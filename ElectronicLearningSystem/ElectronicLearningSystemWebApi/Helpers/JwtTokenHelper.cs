using ElectronicLearningSystemWebApi.Models.UserModel;
using ElectronicLearningSystemWebApi.Repositories.User;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ElectronicLearningSystemWebApi.Helpers
{
    /// <summary>
    /// Хелпер для генерации и проверки токена пользователя.
    /// </summary>
    public class JwtTokenHelper
    {
        private readonly string _key;
        private readonly string _issuer;
        private readonly string _audience;
        private readonly double _accessTokenLifetime;
        private readonly double _refreshTokenLifetime;

        protected readonly IConfiguration _configuration;
        protected readonly IUserRepository _userRepository;

        public JwtTokenHelper(IConfiguration configuration, IUserRepository userRepository)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _key = _configuration["Jwt:Key"] ?? throw new ArgumentNullException("Jwt key is null");
            _issuer = _configuration["Jwt:Issuer"] ?? throw new ArgumentNullException("Jwt issuer is null");
            _audience = _configuration["Jwt:Audience"] ?? throw new ArgumentNullException("Jwt audience is null");

            if (!double.TryParse(_configuration["Jwt:AccessTokenLifetime"], out var accessTokenLifetime))
            {
                throw new InvalidOperationException("Invalid Jwt AccessTokenLifetime configuration");
            }
            _accessTokenLifetime = accessTokenLifetime;

            if (!double.TryParse(_configuration["Jwt:RefreshTokenLifetime"], out var refreshTokenLifetime))
            {
                throw new InvalidOperationException("Invalid Jwt RefreshTokenLifetime configuration");
            }
            _refreshTokenLifetime = refreshTokenLifetime;
        }

        /// <summary>
        /// Проверка токена пользователя, совершившего запрос.
        /// </summary>
        /// <param name="token">Токен.</param>
        /// <returns>Пользователь.</returns>
        /// <exception cref="SecurityTokenException">Ошибка проверки токена.</exception>
        public virtual ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(token);

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _issuer,
                ValidAudience = _audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_key)),
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
        public virtual async Task<(string AccessToken, string RefreshToken)> GenerateTokenForUser(UserEntity user)
        {
            ArgumentNullException.ThrowIfNull(user);
            ArgumentException.ThrowIfNullOrWhiteSpace(user.Login);

            var accessToken = GenerateAccessToken(user.Login);
            var refreshToken = GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_refreshTokenLifetime);

            try
            {
                await _userRepository.UpdateRecordAsync(user);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error updating user with new refresh token.", ex);
            }

            return (accessToken, refreshToken);
        }

        /// <summary>
        /// Генерация токена для доступа.
        /// </summary>
        /// <param name="username">Логин пользователя.</param>
        /// <returns>Токен.</returns>
        protected virtual string GenerateAccessToken(string username)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(username, nameof(username));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, username) }),
                Expires = DateTime.UtcNow.AddHours(_accessTokenLifetime),
                Issuer = _issuer,
                Audience = _audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key)), SecurityAlgorithms.HmacSha256)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        /// <summary>
        /// Генерация рефреш токена.
        /// </summary>
        protected virtual string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];

            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }
}