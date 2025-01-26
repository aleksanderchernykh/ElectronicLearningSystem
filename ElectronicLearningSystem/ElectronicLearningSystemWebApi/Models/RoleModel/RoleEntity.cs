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
    }
}
