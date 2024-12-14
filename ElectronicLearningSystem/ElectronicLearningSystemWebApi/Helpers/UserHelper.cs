using ElectronicLearningSystemWebApi.Models.UserModel;
using ElectronicLearningSystemWebApi.Repositories.User;

namespace ElectronicLearningSystemWebApi.Helpers
{
    public class UserHelper(IUserRepository userRepository)
    {
        protected readonly IUserRepository _userRepository = userRepository 
            ?? throw new ArgumentNullException(nameof(userRepository));

        /// <summary>
        /// Выход пользователя из системы.
        /// </summary>
        /// <param name="user">Пользователь.</param>
        public virtual async Task LogoutUserAsync(UserEntity user)
        {
            user.RefreshToken = null;
            user.RefreshTokenExpiryTime = null;
            await userRepository.UpdateRecordAsync(user);
        }

        /// <summary>
        /// Проверка пароля пользователя.
        /// </summary>
        /// <param name="user">Пользователя.</param>
        /// <param name="password">Пароль.</param>
        /// <returns>Логическое значение равенства паролей.</returns>
        /// <exception cref="ArgumentNullException">Передано пустое значение.</exception>
        public virtual bool VerificationPassword(UserEntity user, string password)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(UserEntity), "Передано пустое значение.");
            }

            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentNullException(nameof(password), "Передано пустое значение пароля пользователя.");
            }

            return PasswordHelper.VerifyPassword(user.Password, password);
        }

    }
}
