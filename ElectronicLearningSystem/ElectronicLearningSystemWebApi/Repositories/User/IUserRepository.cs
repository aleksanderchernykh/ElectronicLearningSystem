using ElectronicLearningSystemWebApi.Models.UserModel;
using ElectronicLearningSystemWebApi.Repositories.Base;

namespace ElectronicLearningSystemWebApi.Repositories.User
{
    public interface IUserRepository : IRepository<UserEntity>
    {
        Task<UserEntity> GetUserByLoginAsync(string login);
    }
}