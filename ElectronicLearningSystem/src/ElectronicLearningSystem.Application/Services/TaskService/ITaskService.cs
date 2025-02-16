using ElectronicLearningSystem.Application.Models.TaskModel.Response;

namespace ElectronicLearningSystem.Application.Services.TaskService
{
    /// <summary>
    /// Интерфейс сервиса для работы с заданиями.
    /// </summary>
    public interface ITaskService
    {
        /// <summary>
        /// Получение заданий по текущему пользователю.
        /// </summary>
        Task<IList<TaskRespose>> GetTaskByCurrentUserAsync();

        /// <summary>
        /// Получение всех заданий.
        /// </summary>
        Task<IList<TaskRespose>> GetAllTaskAsync();

        /// <summary>
        /// Получение задания по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор задания. </param>
        Task<TaskRespose> GetTaskByIdAsync(Guid id);
    }
}
