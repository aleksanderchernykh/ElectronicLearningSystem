using ElectronicLearningSystemWebApi.Models.UserModel;
using ElectronicLearningSystemWebApi.Models.GroupModel;

namespace ElectronicLearningSystemWebApi.Models.StudentModel
{
    /// <summary>
    /// Студент.
    /// </summary>
    public class StudentProfile
    {
        /// <summary>
        /// Идентификатор профиля студента.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Идентификатор группы.
        /// </summary>
        public required Group Group { get; set; }
        public Guid GroupId { get; set; }

        /// <summary>
        /// Идентификатор пользователя.
        /// </summary>
        public required User User { get; set; }
        public Guid UserId { get; set; }
    }
}
