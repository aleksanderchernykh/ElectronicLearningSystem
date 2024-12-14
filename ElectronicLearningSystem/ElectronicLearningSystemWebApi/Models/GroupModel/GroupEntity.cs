using ElectronicLearningSystemWebApi.Models.UserModel;

namespace ElectronicLearningSystemWebApi.Models.GroupModel
{
    public class GroupEntity : EntityBase
    {
        /// <summary>
        /// Наименование группы.
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// Куратор.
        /// </summary>
        public UserEntity Tutor { get; set; }
        public Guid TutorId { get; set; }
    }
}
