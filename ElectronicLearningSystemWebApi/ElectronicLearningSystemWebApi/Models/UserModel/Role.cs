namespace ElectronicLearningSystemWebApi.Models.UserModel
{
    /// <summary>
    /// Роль пользователя.
    /// </summary>
    public class Role
    {
        /// <summary>
        /// Идентификатор.
        /// </summary>
        public Guid Id { get; set; }  

        /// <summary>
        /// Наименование
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// Параметр для получения всех пользователей связанных с ролью.
        /// </summary>
        public ICollection<User> Users { get; set; }
    }
}
