using ElectronicLearningSystem.Infrastructure.Models.TaskModel;

namespace ElectronicLearningSystem.Infrastructure.Models.CommentModel
{
    public class CommentEntity : EntityBase
    {
        public required string Text { get; set; }

        public TaskEntity Task { get; set; }
        public Guid TaskId { get; set; }
    }
}
