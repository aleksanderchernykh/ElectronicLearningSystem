using ElectronicLearningSystemWebApi.Models.CommentModel.DTO;
using ElectronicLearningSystemWebApi.Models.CommentModel.Response;

namespace ElectronicLearningSystemWebApi.Services.CommentService
{
    /// <summary>
    /// Интерфейс сервиса для работы с комментариями к задачам.
    /// </summary>
    public interface ICommentService
    {
        /// <summary>
        /// Получение комментариев для задачи.
        /// </summary>
        /// <param name="id">Идентификатор задачи.</param>
        /// <returns>Список комментариев.</returns>
        Task<IList<CommentResponse>> GetCommentsByTaskAsync(Guid id);

        /// <summary>
        /// Создание комментария по задаче.
        /// </summary>
        /// <param name="createCommentDTO">Данные комментария.</param>
        Task CreateCommentAsync(CreateCommentDTO createCommentDTO);
    }
}
