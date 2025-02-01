using AutoMapper;
using ElectronicLearningSystemWebApi.Models.CommentModel.DTO;
using ElectronicLearningSystemWebApi.Models.CommentModel.Entity;
using ElectronicLearningSystemWebApi.Models.CommentModel.Response;
using ElectronicLearningSystemWebApi.Repositories.Base;

namespace ElectronicLearningSystemWebApi.Helpers
{
    /// <summary>
    /// Хелпер для работы с комментариями для задач.
    /// </summary>
    /// <param name="userHelper">Хелпер для работы с пользователями. </param>
    /// <param name="commentRepository">Репозиторий для работы с комментариями. </param>
    /// <param name="mapper">Маппер. </param>
    public class CommentHelper(UserHelper userHelper,
        IRepository<CommentEntity> commentRepository,
        IMapper mapper)
    {
        /// <summary>
        /// Репозиторий для работы с комментариями. 
        /// </summary>
        protected readonly IRepository<CommentEntity> _commentRepository = commentRepository
            ?? throw new ArgumentNullException(nameof(commentRepository));

        /// <summary>
        /// Хелпер для работы с пользователями. 
        /// </summary>
        protected readonly UserHelper _userHelper = userHelper 
            ?? throw new ArgumentNullException(nameof(userHelper));

        /// <summary>
        /// <param name="mapper">Маппер. </param>
        /// </summary>
        protected readonly IMapper _mapper = mapper 
            ?? throw new ArgumentNullException(nameof(mapper));

        /// <summary>
        /// Получение комментариев для задач.
        /// </summary>
        /// <param name="id">Идентификатор задачи. </param>
        /// <returns>Список комментариев.</returns>
        public async Task<IList<CommentResponse>> GetCommentsByTaskAsync(Guid id)
        {
            var comments = await _commentRepository.GetRecordsByQueryAsync(x => x.TaskId == id);
            return _mapper.Map<IList<CommentResponse>>(comments);
        }

        /// <summary>
        /// Создание комментария по задаче.
        /// </summary>
        /// <param name="createCommentDTO">Информация по комментарию. </param>
        internal async Task CreateCommentAsync(CreateCommentDTO createCommentDTO)
        {
            var comment = _mapper.Map<CommentEntity>(createCommentDTO);
            await _commentRepository.AddRecordAsync(comment);
        }
    }
}
