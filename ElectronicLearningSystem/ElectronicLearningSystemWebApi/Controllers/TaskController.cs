using ElectronicLearningSystemWebApi.Helpers.Controller;
using ElectronicLearningSystemWebApi.Models.ErrorModel;
using ElectronicLearningSystemWebApi.Models.RoleModel.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace ElectronicLearningSystemWebApi.Controllers
{
    /// <summary>
    /// Контроллер для работы с заданиями.
    /// </summary>
    /// <param name="taskHelper"></param>
    [Authorize]
    [Route("task")]
    [ApiController]
    public class TaskController(TaskHelper taskHelper) : ControllerBase
    {
        /// <summary>
        /// Хелпер для работы с заданиями.
        /// </summary>
        private readonly TaskHelper _taskHelper = taskHelper 
            ?? throw new ArgumentNullException(nameof(taskHelper));

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
            var tasks = await _taskHelper.GetTaskByCurrentUserAsync();
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
            var tasks = await _taskHelper.GetAllTaskAsync();
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
            var task = await _taskHelper.GetTaskByIdAsync(id);
            return Ok(task);
        }
    }
}
