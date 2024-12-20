using ElectronicLearningSystemWebApi.Enums;
using ElectronicLearningSystemWebApi.Models.TaskModel;
using ElectronicLearningSystemWebApi.Repositories.Base;
using ElectronicLearningSystemWebApi.Repositories.TaskRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ElectronicLearningSystemWebApi.Controllers
{
    [Authorize]
    [Route("task")]
    [ApiController]
    public class TaskController(ITaskRepository taskRepository,
        ILogger<TaskController> logger) : ControllerBase
    {
        private readonly ITaskRepository _taskRepository = taskRepository 
            ?? throw new ArgumentNullException(nameof(taskRepository));

        private readonly ILogger<TaskController> _logger = 
            logger ?? throw new ArgumentNullException(nameof(logger));

        [HttpGet("getmytask")]
        public async Task<IActionResult> GetMyTask()
        {
            try
            {
                return Ok(await _taskRepository.GetTaskByCurrentUserAsync());
            }
            catch (Exception ex)
            {
                _logger.LogError((int)EventLoggerEnum.DataBaseException, ex.ToString());
                return BadRequest();
            }
        }

        [HttpGet("gettasks")]
        public async Task<IActionResult> GetAllTask()
        {
            try
            {
                return Ok(await _taskRepository.GetAllRecordsAsync());
            }
            catch (Exception ex)
            {
                _logger.LogError((int)EventLoggerEnum.DataBaseException, ex.ToString());
                return BadRequest();
            }
        }

        [HttpGet("gettask/{id}")]
        public async Task<IActionResult> GetTaskById(Guid id)
        {
            try
            {
                return Ok(await _taskRepository.GetRecordByIdAsync(id));
            }
            catch (Exception ex)
            {
                _logger.LogError((int)EventLoggerEnum.DataBaseException, ex.ToString());
                return BadRequest();
            }
        }
    }
}
