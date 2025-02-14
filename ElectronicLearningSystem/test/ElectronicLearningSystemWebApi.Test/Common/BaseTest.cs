using ElectronicLearningSystemWebApi.Repositories.User;
using Moq;
using ElectronicLearningSystemWebApi.Services.UserService;
using ElectronicLearningSystemWebApi.Helpers.JwtTokenHelper;
using ElectronicLearningSystemWebApi.Helpers.RedisHelper;
using ElectronicLearningSystemWebApi.Helpers.EmailSendingHelper;

namespace ElectronicLearningSystemWebApi.Test.Common
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
