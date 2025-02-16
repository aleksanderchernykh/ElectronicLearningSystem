using ElectronicLearningSystem.Core.CustomException;
using ElectronicLearningSystem.Infrastructure.Context;
using ElectronicLearningSystem.Infrastructure.Models.UserModel;
using ElectronicLearningSystem.Infrastructure.Repositories.Base;

namespace ElectronicLearningSystem.Infrastructure.Repositories.User
{
    /// <summary>
    /// ctor.
    /// </summary>
    /// <param name="applicationContext">Контекст работы с БД.</param>
    public class UserRepository(ApplicationContext context)
        : RepositoryBase<UserEntity>(context), IUserRepository
    {
        /// <summary>
        /// Получение пользователя по логину.
        /// </summary>
        /// <param name="login">Логин.</param>
        /// <returns>Пользователь.</returns>
        public async Task<UserEntity?> GetUserByLoginAsync(string login)
        {
            if (string.IsNullOrEmpty(login))
                return null;

            return await GetFirstRecordsByQueryAsync(x => x.Login == login);
        }

        /// <summary>
        /// Создание нового пользователя.
        /// </summary>
        /// <param name="userResponse">Данные пользователя.</param>
        /// <exception cref="DublicateUserException">Пользователь с переданным логином уже существует.</exception>
        /// <exception cref="ArgumentNullException">Передано пустое значение логина или пароля.</exception>
        public async System.Threading.Tasks.Task CreateUser()
        {
            // TODO
            /* var dublicateUser = await GetUserByLoginAsync(userResponse.Login);

            if (dublicateUser != null)
                throw new DublicateUserException("Пользователь с переданным логином уже существует.");

            if (string.IsNullOrEmpty(userResponse.Login) ||
                string.IsNullOrEmpty(userResponse.Password))
                throw new ArgumentNullException(nameof(userResponse), "Передано пустое значение логина или пароля.");

            var user = new UserEntity()
            {
                Email = userResponse.Email,
                Password = PasswordHelper.HashPassword(userResponse.Password),
                RoleId = userResponse.RoleId,
                Login = userResponse.Login,
            };
            
            await AddRecordAsync(user); */
        }
    }
}