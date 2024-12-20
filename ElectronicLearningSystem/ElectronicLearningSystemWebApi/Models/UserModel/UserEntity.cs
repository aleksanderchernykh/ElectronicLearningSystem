using ElectronicLearningSystemWebApi.Models.GroupModel;
using ElectronicLearningSystemWebApi.Models.NotificationModel;
using ElectronicLearningSystemWebApi.Models.RoleModel;
using ElectronicLearningSystemWebApi.Models.StudentModel;
using ElectronicLearningSystemWebApi.Models.TaskModel;
using Microsoft.Identity.Client;
using System.Data;
using System.Text.RegularExpressions;
using GroupEntity = ElectronicLearningSystemWebApi.Models.GroupModel.GroupEntity;

namespace ElectronicLearningSystemWebApi.Models.UserModel
{
    /// <summary>
    /// Пользователь.
    /// </summary>
    public class UserEntity : EntityBase
    {
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
        public RoleEntity Role { get; set; }

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
        public StudentProfileEntity? StudentProfile { get; set; }

        /// <summary>
        /// Навигационное свойство для групп.
        /// </summary>
        public IEnumerable<GroupEntity>? Groups { get; set; }

        /// <summary>
        /// Оповещения.
        /// </summary>
        public IEnumerable<NotificationEntity>? Notifications { get; set; }
    }
}
