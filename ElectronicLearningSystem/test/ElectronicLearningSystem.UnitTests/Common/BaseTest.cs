using ElectronicLearningSystemWebApi.Helpers.EmailSendingHelper;
using ElectronicLearningSystemWebApi.Helpers.JwtTokenHelper;
using ElectronicLearningSystemWebApi.Helpers.RedisHelper;
using ElectronicLearningSystemWebApi.Repositories.User;
using ElectronicLearningSystemWebApi.Services.UserService;
using Moq;

namespace ElectronicLearningSystem.UnitTests.Common
{
    public abstract class BaseTest
    {
        protected readonly Mock<IUserRepository> _userRepositoryMock = new();
        protected readonly Mock<IUserService> _userServiceMock = new();
        protected readonly Mock<IJwtTokenHelper> _tokenHelperMock = new();
        protected readonly Mock<IEmailSendingHelper> _emailSendingHelperMock = new();
        protected readonly Mock<IRedisHelper> _redisHelperMock = new();
    }
}
