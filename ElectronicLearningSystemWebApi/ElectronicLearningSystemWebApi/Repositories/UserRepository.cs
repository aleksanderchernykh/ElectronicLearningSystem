using ElectronicLearningSystemWebApi.Context;
using ElectronicLearningSystemWebApi.Helpers;
using ElectronicLearningSystemWebApi.Helpers.CustomException;
using ElectronicLearningSystemWebApi.Models.UserModel;
using ElectronicLearningSystemWebApi.Models.UserModel.Response;
using Microsoft.EntityFrameworkCore;

namespace ElectronicLearningSystemWebApi.Repositories
{
    /// <summary>
    /// ctor.
    /// </summary>
    /// <param name="applicationContext">Контекст работы с БД.</param>
    public class UserRepository(ApplicationContext applicationContext)
    {
        /// <summary>
        /// Контекст работы с БД.
        /// </summary>
        private readonly ApplicationContext _applicationContext = applicationContext;

        /// <summary>
        /// Получение пользователя по логину.
        /// </summary>
        /// <param name="login">Логин.</param>
        /// <returns>Пользователь.</returns>
        public async Task<User> GetUserByLoginAsync(string login)
        {
            if (string.IsNullOrEmpty(login))
            {
                return null;
            }

            return await _applicationContext.User.FirstOrDefaultAsync(x => x.Login == login);
        }

        /// <summary>
        /// Проверка пароля пользователя.
        /// </summary>
        /// <param name="user">Пользователя.</param>
        /// <param name="password">Пароль.</param>
        /// <returns>Логическое значение равенства паролей.</returns>
        /// <exception cref="ArgumentNullException">Передано пустое значение.</exception>
        public bool VerificationPassword(User user, string password)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(User), "Передано пустое значение.");
            }

            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentNullException(nameof(password), "Передано пустое значение пароля пользователя.");
            }

            return PasswordHelper.VerifyPassword(user.Password, password);
        }

        /// <summary>
        /// Обновление данных пользователя.
        /// </summary>
        /// <param name="user">Пользователь.</param>
        /// <exception cref="ArgumentNullException">Передано пустое значение.</exception>
        public void UpdateUser(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(User), "Передано пустое значение.");
            }

            _applicationContext.User.Update(user);
        }

        /// <summary>
        /// Создание нового пользователя.
        /// </summary>
        /// <param name="userResponse">Данные пользователя.</param>
        /// <exception cref="DublicateUserException">Пользователь с переданным логином уже существует.</exception>
        /// <exception cref="ArgumentNullException">Передано пустое значение логина или пароля.</exception>
        public async Task CreateUser(CreateUserRequest userResponse)
        {
            var dublicateUser = await GetUserByLoginAsync(userResponse.Login);

            if (dublicateUser != null)
            {
                throw new DublicateUserException("Пользователь с переданным логином уже существует.");
            }

            if (string.IsNullOrEmpty(userResponse.Login) ||
                string.IsNullOrEmpty(userResponse.Password))
            {
                throw new ArgumentNullException(nameof(userResponse), "Передано пустое значение логина или пароля.");
            }

            var user = new User()
            {
                Email = userResponse.Email,
                Password = PasswordHelper.HashPassword(userResponse.Password),
                RoleId = userResponse.RoleId,
                Login = userResponse.Login,
            };

            _applicationContext.User.Add(user);
            _applicationContext.SaveChanges();
        }

        /// <summary>
        /// Получение списка ролей пользователя.
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Role>> GetRoles()
        {
            return await _applicationContext.Role.ToListAsync();
        }

        /// <summary>
        /// Получение пользователя по ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Пользователь.</returns>
        public async Task<User?> GetUserByIdAsync(Guid id)
        {
            return await _applicationContext.User.FirstOrDefaultAsync(x => x.Id == id);
        }

        /// <summary>
        /// Получение всех пользователей.
        /// </summary>
        /// <returns>Все пользователи.</returns>
        public async Task<IEnumerable<User>> GetUsers()
        {
            return await _applicationContext.User.ToListAsync();
        }

        /// <summary>
        /// Выход пользователя из системы.
        /// </summary>
        /// <param name="user">Пользователь.</param>
        public void LogoutUser(User user)
        {
            user.RefreshToken = null;
            user.RefreshTokenExpiryTime = null;
            _applicationContext.Update(user);
            _applicationContext.SaveChanges();
        }
    }
}