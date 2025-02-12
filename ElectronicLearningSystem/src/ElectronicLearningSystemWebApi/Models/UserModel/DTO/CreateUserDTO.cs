namespace ElectronicLearningSystemWebApi.Models.UserModel.DTO
{
    /// <summary>
    /// Заспрос на создание нового пользователя.
    /// </summary>
    public class CreateUserDTO : UserLoginDTO
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
