using ElectronicLearningSystem.Infrastructure.Models.UserModel;

namespace ElectronicLearningSystem.Infrastructure.Models.TaskModel
{
    public class TaskEntity : EntityBase
    {
        public required string Topic { get; set; }

        public required string Description { get; set; }

        public required UserEntity Owner { get; set; }
        public Guid OwnerId { get; set; }

        public required UserEntity Student { get; set; }
        public Guid StudentId { get; set; }
    }
}
