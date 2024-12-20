using ElectronicLearningSystemWebApi.Models.UserModel;

namespace ElectronicLearningSystemWebApi.Models.RoleModel
{
    /// <summary>
    /// Роль пользователя.
    /// </summary>
    public class RoleEntity : EntityBase
    {
        /// <summary>
        /// Наименование.
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// Параметр для получения всех пользователей связанных с ролью.
        /// </summary>
        public ICollection<UserEntity> Users { get; set; }
    }
}
