using ElectronicLearningSystemWebApi.Enums;
using ElectronicLearningSystemWebApi.Models.UserModel;
using ElectronicLearningSystemWebApi.Repositories.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ElectronicLearningSystemWebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController(IRepository<RoleEntity> roleRepository,
        ILogger<RoleController> logger) 
        : ControllerBase
    {
        private readonly IRepository<RoleEntity> _roleRepository = 
            roleRepository ?? throw new ArgumentNullException(nameof(logger));

        private readonly ILogger<RoleController> _logger = 
            logger ?? throw new ArgumentNullException(nameof(logger));

        /// <summary>
        /// Получение ролей пользователя.
        /// </summary>
        /// <returns></returns>
        [HttpGet("getroles")]
        public async Task<IActionResult> GetRoles()
        {
            try
            {
                return Ok(await _roleRepository.GetAllRecordAsync());
            }
            catch (Exception ex)
            {
                _logger.LogError(new EventId((int)EventLoggerEnum.DataBaseException), message: ex.ToString());
                return BadRequest(ex.Message);
            }
        }
    }
}
