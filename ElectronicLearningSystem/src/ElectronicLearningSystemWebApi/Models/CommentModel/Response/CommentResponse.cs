namespace ElectronicLearningSystemWebApi.Models.CommentModel.Response
{
    public class CommentResponse : BaseResponse
    {
        public string Text { get; set; }

        public Guid TaskId { get; set; }
    }
}
