using ElectronicLearningSystemWebApi.Models.UserModel;

namespace ElectronicLearningSystemWebApi.Models.GroupModel
{
    public class Group
    {
        /// <summary>
        /// Идентификатор.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Наименование группы.
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// Куратор.
        /// </summary>
        public User Tutor { get; set; }
        public Guid TutorId { get; set; }
    }
}
