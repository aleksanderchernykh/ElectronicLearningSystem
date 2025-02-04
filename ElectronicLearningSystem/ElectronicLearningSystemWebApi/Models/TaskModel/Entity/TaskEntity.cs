using ElectronicLearningSystemWebApi.Models.UserModel.Entity;

namespace ElectronicLearningSystemWebApi.Models.TaskModel.Entity
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
