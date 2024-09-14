using ElectronicLearningSystemWebApi.Models.GroupModel;
using ElectronicLearningSystemWebApi.Models.StudentModel;
using Microsoft.Identity.Client;
using System.Data;
using System.Text.RegularExpressions;
using Group = ElectronicLearningSystemWebApi.Models.GroupModel.Group;

namespace ElectronicLearningSystemWebApi.Models.UserModel
{
    /// <summary>
    /// Пользователь.
    /// </summary>
    public class User
    {
        /// <summary>
        /// Идентификатор.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Имя.
        /// </summary>
        public string? FirstName { get; set; }

        /// <summary>
        /// Фамилия.
        /// </summary>
        public string? LastName { get; set; }

        /// <summary>
        /// Отчество.
        /// </summary>
        public string? Patronymic { get; set; }

        /// <summary>
        /// Почта.
        /// </summary>
        public required string Email { get; set; }

        /// <summary>
        /// Логин пользователя.
        /// </summary>
        public required string Login {  get; set; }

        /// <summary>
        /// Пароль.
        /// </summary>
        public required string Password { get; set; }

        /// <summary>
        /// Роль пользователя.
        /// </summary>
        public Guid RoleId { get; set; }
        public Role Role { get; set; }

        /// <summary>
        /// Токен для обновления.
        /// </summary>
        public string? RefreshToken { get; set; }

        /// <summary>
        /// Время работы токена.
        /// </summary>
        public DateTime? RefreshTokenExpiryTime { get; set; }

        /// <summary>
        /// Заблокирована учетная запись
        /// </summary>
        public bool IsLocked { get; set; } = false;

        /// <summary>
        /// Навигационное свойство для профиля студента.
        /// </summary>
        public StudentProfile? StudentProfile { get; set; }

        /// <summary>
        /// Навигационное свойство для групп.
        /// </summary>
        public IEnumerable<Group>? Groups { get; set; }
    }
}
