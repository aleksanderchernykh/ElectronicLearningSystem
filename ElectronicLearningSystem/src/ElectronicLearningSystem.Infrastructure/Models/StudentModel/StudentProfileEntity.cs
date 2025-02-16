using ElectronicLearningSystem.Infrastructure.Models.GroupModel;
using ElectronicLearningSystem.Infrastructure.Models.UserModel;

namespace ElectronicLearningSystem.Infrastructure.Models.StudentModel
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
