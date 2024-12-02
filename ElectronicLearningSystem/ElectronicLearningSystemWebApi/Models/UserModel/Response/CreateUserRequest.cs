namespace ElectronicLearningSystemWebApi.Models.UserModel.Response
{
    /// <summary>
    /// Заспрос на создание нового пользователя.
    /// </summary>
    public class CreateUserRequest : UserLoginRequest
    {
        /// <summary>
        /// Почта.
        /// </summary>
        public required string Email { get; set; }

        /// <summary>
        /// Роль пользователя.
        /// </summary>
        public required Guid RoleId { get; set; }
    }
}
