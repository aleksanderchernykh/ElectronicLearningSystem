using AutoMapper;
using ElectronicLearningSystemWebApi.Models.TaskModel.Response;
using ElectronicLearningSystemWebApi.Repositories.TaskRepository;

namespace ElectronicLearningSystemWebApi.Helpers.Services.TaskService
{
    /// <summary>
    /// Хелпер для работы с заданиями.
    /// </summary>
    /// <param name="taskRepository">Хелпер для работы с заданиями. </param>
    /// <param name="mapper">Маппер. </param>
    public class TaskService(ITaskRepository taskRepository,
        IMapper mapper) : ITaskService
    {
        /// <summary>
        /// Хелпер для работы с заданиями.
        /// </summary>
        protected readonly ITaskRepository _taskRepository = taskRepository
            ?? throw new ArgumentNullException(nameof(taskRepository));

        /// <summary>
        /// Маппер.
        /// </summary>
        protected readonly IMapper _mapper = mapper
            ?? throw new ArgumentNullException(nameof(mapper));

        /// <summary>
        /// Получение заданий по текущему пользователю.
        /// </summary>
        public async Task<IList<TaskRespose>> GetTaskByCurrentUserAsync()
        {
            var tasks = await _taskRepository.GetTaskByCurrentUserAsync();
            return _mapper.Map<IList<TaskRespose>>(tasks);
        }

        /// <summary>
        /// Получение всех заданий.
        /// </summary>
        public async Task<IList<TaskRespose>> GetAllTaskAsync()
        {
            var tasks = await _taskRepository.GetAllRecordsAsync();
            return _mapper.Map<IList<TaskRespose>>(tasks);
        }

        /// <summary>
        /// Получение задания по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор задания. </param>
        public async Task<TaskRespose> GetTaskByIdAsync(Guid id)
        {
            var task = await _taskRepository.GetRecordByIdAsync(id)
                ?? throw new ArgumentNullException($"The task with id: {id} was not found");

            return mapper.Map<TaskRespose>(task);
        }
    }
}
