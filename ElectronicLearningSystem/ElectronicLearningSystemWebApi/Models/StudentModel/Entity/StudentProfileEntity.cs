using ElectronicLearningSystemWebApi.Models.GroupModel.Entity;
using ElectronicLearningSystemWebApi.Models.UserModel.Entity;

namespace ElectronicLearningSystemWebApi.Models.StudentModel.Entity
{
    /// <summary>
    /// Студент.
    /// </summary>
    public class StudentProfileEntity : EntityBase
    {
        /// <summary>
        /// Идентификатор группы.
        /// </summary>
        public required GroupEntity Group { get; set; }
        public Guid GroupId { get; set; }

        /// <summary>
        /// Идентификатор пользователя.
        /// </summary>
        public required UserEntity User { get; set; }
        public Guid UserId { get; set; }
    }
}
