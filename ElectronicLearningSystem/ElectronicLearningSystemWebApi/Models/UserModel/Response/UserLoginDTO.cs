namespace ElectronicLearningSystemWebApi.Models.UserModel.Response
{
    /// <summary>
    /// Запрос на авторизацию пользователя.
    /// </summary>
    public class UserLoginDTO
    {
        /// <summary>
        /// Логин пользователя.
        /// </summary>
        public required string Login { get; set; }

        /// <summary>
        /// Пароль пользователя.
        /// </summary>
        public required string Password { get; set; }
    }
}
