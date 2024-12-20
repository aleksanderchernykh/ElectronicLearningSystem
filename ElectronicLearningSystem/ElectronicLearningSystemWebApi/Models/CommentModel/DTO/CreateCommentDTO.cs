namespace ElectronicLearningSystemWebApi.Models.CommentModel.DTO
{
    public class CreateCommentDTO
    {
        public required string Text { get; set; }

        public required Guid TaskId { get; set; }
    }
}
