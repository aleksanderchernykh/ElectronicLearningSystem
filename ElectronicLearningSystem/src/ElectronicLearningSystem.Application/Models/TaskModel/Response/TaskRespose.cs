namespace ElectronicLearningSystem.Application.Models.TaskModel.Response
{
    public class TaskRespose : BaseResponse
    {
        public string Topic { get; set; }

        public string Description { get; set; }

        public Guid OwnerId { get; set; }

        public Guid StudentId { get; set; }
    }
}
