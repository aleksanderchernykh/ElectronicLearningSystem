using ElectronicLearningSystemWebApi.Helpers.Services.TaskService;
using ElectronicLearningSystemWebApi.Models.ErrorModel;
using ElectronicLearningSystemWebApi.Models.RoleModel.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace ElectronicLearningSystemWebApi.Controllers
{
    /// <summary>
    /// Контроллер для работы с заданиями.
    /// </summary>
    /// <param name="taskService"></param>
    [Authorize]
    [Route("task")]
    [ApiController]
    public class TaskController(ITaskService taskService) : ControllerBase
    {
        /// <summary>
        /// Хелпер для работы с заданиями.
        /// </summary>
        private readonly ITaskService _taskService = taskService 
            ?? throw new ArgumentNullException(nameof(taskService));

        /// <summary>
        /// Получение всех задач, связанных с текущим пользователем.
        /// </summary>
        /// <response code="200">Успешный возврат задач, связанных с текущим пользователем. </response>
        /// <response code="500">Ошибка сервера. </response>
        [Produces("application/json")]
        [ProducesResponseType(typeof(RoleResponse), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        [HttpGet("getmytask")]
        public async Task<IActionResult> GetTaskByCurrentUser()
        {
            var tasks = await _taskService.GetTaskByCurrentUserAsync();
            return Ok(tasks);
        }

        /// <summary>
        /// Получение всех заданий.
        /// </summary>
        /// <response code="200">Успешный возврат всех задач. </response>
        /// <response code="500">Ошибка сервера. </response>
        [Produces("application/json")]
        [ProducesResponseType(typeof(RoleResponse), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        [HttpGet("get")]
        public async Task<IActionResult> GetAllTask()
        {
            var tasks = await _taskService.GetAllTaskAsync();
            return Ok(tasks);
        }

        /// <summary>
        /// Получение задания по идентификатору.
        /// </summary>
        /// <response code="200">Успешный возврат задачи по идентификатору. </response>
        /// <response code="500">Ошибка сервера. </response>
        [Produces("application/json")]
        [ProducesResponseType(typeof(RoleResponse), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetTaskById(Guid id)
        {
            var task = await _taskService.GetTaskByIdAsync(id);
            return Ok(task);
        }
    }
}
