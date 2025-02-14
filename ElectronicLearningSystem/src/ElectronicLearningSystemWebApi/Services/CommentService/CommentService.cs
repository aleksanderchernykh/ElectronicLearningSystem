using AutoMapper;
using ElectronicLearningSystemWebApi.Models.CommentModel.DTO;
using ElectronicLearningSystemWebApi.Models.CommentModel.Entity;
using ElectronicLearningSystemWebApi.Models.CommentModel.Response;
using ElectronicLearningSystemWebApi.Repositories.Base;

namespace ElectronicLearningSystemWebApi.Services.CommentService
{
    /// <summary>
    /// Хелпер для работы с комментариями для задач.
    /// </summary>
    /// <param name="userHelper">Хелпер для работы с пользователями. </param>
    /// <param name="commentRepository">Репозиторий для работы с комментариями. </param>
    /// <param name="mapper">Маппер. </param>
    public class CommentService(IRepository<CommentEntity> commentRepository,
        IMapper mapper) : ICommentService
    {
        /// <summary>
        /// Репозиторий для работы с комментариями. 
        /// </summary>
        protected readonly IRepository<CommentEntity> _commentRepository = commentRepository
            ?? throw new ArgumentNullException(nameof(commentRepository));

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
        public async Task CreateCommentAsync(CreateCommentDTO createCommentDTO)
        {
            var comment = _mapper.Map<CommentEntity>(createCommentDTO);
            await _commentRepository.AddRecordAsync(comment);
        }
    }
}
