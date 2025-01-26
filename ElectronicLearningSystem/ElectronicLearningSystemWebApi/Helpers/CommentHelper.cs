using ElectronicLearningSystemWebApi.Models.CommentModel;
using ElectronicLearningSystemWebApi.Models.CommentModel.DTO;
using ElectronicLearningSystemWebApi.Repositories.User;

namespace ElectronicLearningSystemWebApi.Helpers
{
    public class CommentHelper(UserHelper userHelper)
    {
        protected readonly UserHelper _userHelper = userHelper 
            ?? throw new ArgumentNullException(nameof(userHelper));

        public virtual CommentEntity GetCommentByDTO(CreateCommentDTO createCommentDTO)
        {
            return new CommentEntity 
            { 
                Text = createCommentDTO.Text,
                TaskId = createCommentDTO.TaskId,
                CreatedOn = DateTime.UtcNow,
                ModifiedOn = DateTime.UtcNow,
                CreatedById = _userHelper.GetCurrentUserId(),
                ModifiedById = _userHelper.GetCurrentUserId()
            };
        }
    }
}
