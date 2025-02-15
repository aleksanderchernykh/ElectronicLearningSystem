using ElectronicLearningSystem.Infrastructure.Models.UserModel;
using ElectronicLearningSystem.Infrastructure.Repositories.Base;

namespace ElectronicLearningSystem.Infrastructure.Repositories.User
{
    public interface IUserRepository : IRepository<UserEntity>
    {
        Task<UserEntity?> GetUserByLoginAsync(string login);
    }
}