using ElectronicLearningSystem.Application.Services;
using ElectronicLearningSystem.Common.Helpers.EmailSendingHelper;
using ElectronicLearningSystem.Common.Helpers.JwtTokenHelper;
using ElectronicLearningSystem.Common.Helpers.RedisHelper;
using ElectronicLearningSystem.Infrastructure.Repositories.User;
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
