using ElectronicLearningSystem.Application.Models.UserModel.DTO;
using ElectronicLearningSystem.Application.Services.AuthService;
using ElectronicLearningSystem.Core.Helpers;
using ElectronicLearningSystem.Infrastructure.Models.UserModel;
using ElectronicLearningSystem.UnitTests.Common;
using Moq;
using System.Security.Claims;
using Xunit;

namespace ElectronicLearningSystem.UnitTests.ServicesTests
{
    public class AuthServiceTests : BaseTest
    {
        private readonly IAuthService _authService;
        private readonly UserEntity _userEntity;

        public AuthServiceTests()
        {
            _authService = new AuthService(
                _userRepositoryMock.Object,
                _userServiceMock.Object,
                _tokenHelperMock.Object,
                _emailSendingHelperMock.Object,
                _redisHelperMock.Object
            );

            _userEntity = new UserEntity
            {
                Id = Guid.NewGuid(),
                Login = "existentUser",
                Password = PasswordHelper.HashPassword("correctPassword"),
                IsLocked = false,
                Email = "random@mail.ru",
                RefreshToken = "valid_refresh_token"
            };
        }

        #region LoginAsync

        [Fact]
        public async Task LoginAsync_ShouldThrowException_WhenUserNotFound()
        {
            // Arrange
            var loginDto = new UserLoginDTO
            {
                Login = "nonExistentUser",
                Password = "correctPassword"
            };

            _userRepositoryMock.Setup(repo => repo.GetUserByLoginAsync(loginDto.Login))
                .ReturnsAsync((UserEntity)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _authService.LoginAsync(loginDto));

            Assert.Equal("The user entered the wrong username", exception.Message);
        }

        [Fact]
        public async Task LoginAsync_ShouldThrowException_IncorrectPassword()
        {
            // Arrange
            var loginDto = new UserLoginDTO
            {
                Login = "existentUser",
                Password = "incorrectPassword"
            };

            _userRepositoryMock.Setup(repo => repo.GetUserByLoginAsync(loginDto.Login))
                .ReturnsAsync(_userEntity);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _authService.LoginAsync(loginDto));

            Assert.Equal("The user entered the wrong username or password", exception.Message);
        }

        [Fact]
        public async Task LoginAsync_ShouldThrowException_LockerUser()
        {
            // Arrange
            var loginDto = new UserLoginDTO
            {
                Login = "existentUser",
                Password = "correctPassword"
            };

            var lockerUser = new UserEntity
            {
                Id = Guid.NewGuid(),
                Login = "existentUser",
                Password = PasswordHelper.HashPassword("correctPassword"),
                IsLocked = true,
                Email = "random@mail.ru"
            };

            _userRepositoryMock.Setup(repo => repo.GetUserByLoginAsync(loginDto.Login))
                .ReturnsAsync(lockerUser);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _authService.LoginAsync(loginDto));

            Assert.Equal("The user's account has been deactivated", exception.Message);
        }

        [Fact]
        public async Task LoginAsync_ShouldReturnTokens_WhenCredentialsAreCorrect()
        {
            // Arrange
            var loginDto = new UserLoginDTO 
            { 
                Login = "existentUser", 
                Password = "correctPassword"
            };

            _userRepositoryMock.Setup(repo => repo.GetUserByLoginAsync(loginDto.Login))
                .ReturnsAsync(_userEntity);

            _tokenHelperMock.Setup(token => token.GenerateTokenForUser(_userEntity))
                .ReturnsAsync(("accessToken", "refreshToken"));

            _authService.VerificationPassword(_userEntity, loginDto.Password);

            // Act
            var result = await _authService.LoginAsync(loginDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("accessToken", result.AccessToken);
            Assert.Equal("refreshToken", result.RefreshToken);
        }

        #endregion

        #region LogoutAsync

        [Fact]
        public async Task LogoutAsync_ShouldThrowKeyNotFoundException_WhenUserNotFound()
        {
            // Arrange
            var logoutDTO = new LogoutDTO 
            { 
                Login = "nonexistentUser" 
            };

            _userRepositoryMock
                .Setup(repo => repo.GetUserByLoginAsync(logoutDTO.Login))
                .ReturnsAsync((UserEntity)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(async () =>
                await _authService.LogoutAsync(logoutDTO));
        }

        [Fact]
        public async Task LogoutAsync_ShouldCallLogoutUserAsync_WhenUserFound()
        {
            // Arrange
            var logoutDTO = new LogoutDTO 
            { 
                Login = _userEntity.Login,
            };

            _userRepositoryMock
                .Setup(repo => repo.GetUserByLoginAsync(logoutDTO.Login))
                .ReturnsAsync(_userEntity);

            await _authService.LogoutAsync(logoutDTO);
        }

        #endregion

        #region RefreshTokenAsync

        [Fact]
        public async Task RefreshTokenAsync_ShouldThrowUnauthorizedAccessException_WhenTokenIsInvalid()
        {
            // Arrange
            var request = new RefreshTokenDTO 
            {
                AccessToken = "invalid_token", 
                RefreshToken = "refresh_token" 
            };

            _tokenHelperMock
                .Setup(helper => helper.GetPrincipalFromExpiredToken(request.AccessToken))
                .Returns((ClaimsPrincipal)null);

            // Act & Assert
            await Assert.ThrowsAsync<UnauthorizedAccessException>(async () =>
                await _authService.RefreshTokenAsync(request));
        }

        [Fact]
        public async Task RefreshTokenAsync_ShouldThrowUnauthorizedAccessException_WhenUserNotFound()
        {
            // Arrange
            var request = new RefreshTokenDTO 
            { 
                AccessToken = "valid_token", 
                RefreshToken = "refresh_token" 
            };

            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, "testUser")
            }));

            _tokenHelperMock
                .Setup(helper => helper.GetPrincipalFromExpiredToken(request.AccessToken))
                .Returns(claimsPrincipal);

            _userRepositoryMock
                .Setup(repo => repo.GetUserByLoginAsync("testUser"))
                .ReturnsAsync((UserEntity)null);

            // Act & Assert
            await Assert.ThrowsAsync<UnauthorizedAccessException>(async () =>
                await _authService.RefreshTokenAsync(request));
        }

        [Fact]
        public async Task RefreshTokenAsync_ShouldThrowUnauthorizedAccessException_WhenRefreshTokenIsInvalid()
        {
            // Arrange
            var request = new RefreshTokenDTO 
            { 
                AccessToken = "valid_token", 
                RefreshToken = "wrong_refresh_token" 
            };

            var user = new UserEntity
            {
                Id = Guid.NewGuid(),
                Login = "existentUser",
                Password = PasswordHelper.HashPassword("correctPassword"),
                IsLocked = false,
                Email = "random@mail.ru",
                RefreshToken = "incorrectToken"
            };

            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, user.Login)
            }));

            _tokenHelperMock
                .Setup(helper => helper.GetPrincipalFromExpiredToken(request.AccessToken))
                .Returns(claimsPrincipal);

            _userRepositoryMock
                .Setup(repo => repo.GetUserByLoginAsync(user.Login))
                .ReturnsAsync(user);

            // Act & Assert
            await Assert.ThrowsAsync<UnauthorizedAccessException>(async () =>
                await _authService.RefreshTokenAsync(request));
        }

        [Fact]
        public async Task RefreshTokenAsync_ShouldReturnNewTokens_WhenCredentialsAreValid()
        {
            // Arrange
            var request = new RefreshTokenDTO 
            { 
                AccessToken = "valid_token", 
                RefreshToken = "valid_refresh_token" 
            };

            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, _userEntity.Login)
            }));

            _tokenHelperMock
                .Setup(helper => helper.GetPrincipalFromExpiredToken(request.AccessToken))
                .Returns(claimsPrincipal);

            _userRepositoryMock
                .Setup(repo => repo.GetUserByLoginAsync(_userEntity.Login))
                .ReturnsAsync(_userEntity);

            _tokenHelperMock
                .Setup(helper => helper.GenerateTokenForUser(_userEntity))
                .ReturnsAsync(("new_access_token", "new_refresh_token"));

            // Act
            var result = await _authService.RefreshTokenAsync(request);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("new_access_token", result.AccessToken);
            Assert.Equal("new_refresh_token", result.RefreshToken);
        }

        #endregion

        #region RecoveryPasswordAsync

        [Fact]
        public async Task RecoveryPasswordAsync_ShouldThrowKeyNotFoundException_WhenUserNotFound()
        {
            // Arrange
            var recoveryPasswordDTO = new RecoveryPasswordDTO { 
                Login = "nonexistentUser" 
            };

            _userRepositoryMock
                .Setup(repo => repo.GetUserByLoginAsync(recoveryPasswordDTO.Login))
                .ReturnsAsync((UserEntity)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(async () =>
                await _authService.RecoveryPasswordAsync(recoveryPasswordDTO));
        }

        [Fact]
        public async Task RecoveryPasswordAsync_ShouldCallRedisAndEmailMethods_WhenUserFound()
        {
            // Arrange
            var recoveryPasswordDTO = new RecoveryPasswordDTO 
            { 
                Login = _userEntity.Login 
            };

            _userRepositoryMock
                .Setup(repo => repo.GetUserByLoginAsync(recoveryPasswordDTO.Login))
                .ReturnsAsync(_userEntity);

            _redisHelperMock
                .Setup(redis => redis.RecoveryPasswordAsync(It.IsAny<string>(), _userEntity.Id, It.IsAny<TimeSpan>()))
                .Returns(Task.CompletedTask);

            _emailSendingHelperMock
                .Setup(email => email.SendRecoveryPasswordAsync(_userEntity, It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            // Act
            await _authService.RecoveryPasswordAsync(recoveryPasswordDTO);

            // Assert
            _redisHelperMock.Verify(redis =>
                redis.RecoveryPasswordAsync(It.IsAny<string>(), _userEntity.Id, It.IsAny<TimeSpan>()), Times.Once);

            _emailSendingHelperMock.Verify(email =>
                email.SendRecoveryPasswordAsync(_userEntity, It.IsAny<string>()), Times.Once);
        }

        #endregion

        #region GetUserIdByRecoveryPasswordTokenAsync

        [Fact]
        public async Task GetUserIdByRecoveryPasswordTokenAsync_ShouldThrowArgumentNullException_WhenNotDataFromToken()
        {
            // Arrange
            var invalidToken = Guid.NewGuid();

            _redisHelperMock.Setup(repo => repo.GetUserIdByRecoveryPasswordTokenAsync(Guid.NewGuid()))
                .ReturnsAsync((Guid?)null);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => 
                await _authService.GetUserIdByRecoveryPasswordTokenAsync(invalidToken));
        }

        #endregion

        #region VerificationPassword

        [Fact]
        public void VerificationPassword_ShouldThrowArgumentNullException_UserThrowIfNull()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _authService.VerificationPassword((UserEntity)null, "password"));
        }

        [Fact]
        public void VerificationPassword_ShouldThrowArgumentNullException_PasswordThrowIfNull()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _authService.VerificationPassword(_userEntity, null));
        }

        [Fact]
        public void VerificationPassword_ShouldReturnFalse_IncorrectPassword()
        {
            // Act & Assert
            Assert.False(_authService.VerificationPassword(_userEntity, "incorrectPassword"));
        }

        [Fact]
        public void VerificationPassword_ShouldReturnTrue_CorrectPassword()
        {
            // Act & Assert
            Assert.True(_authService.VerificationPassword(_userEntity, "correctPassword"));
        }

        #endregion
    }
}