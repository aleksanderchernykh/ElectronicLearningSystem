using ElectronicLearningSystemWebApi.Models.ErrorModel;
using ElectronicLearningSystemWebApi.Models.RoleModel.Response;
using ElectronicLearningSystemWebApi.Services.RoleService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ElectronicLearningSystemWebApi.Controllers
{
    /// <summary>
    /// Контроллер для работы с ролями.
    /// </summary>
    /// <param name="roleService">Хелпер для работы с ролями. </param>
    [Authorize]
    [Route("role")]
    [ApiController]
    public class RoleController(IRoleService roleService) 
        : ControllerBase
    {
        /// <summary>
        /// Хелпер для работы с ролями.
        /// </summary>
        private readonly IRoleService _roleService = roleService 
            ?? throw new ArgumentNullException(nameof(roleService));

        /// <summary>
        /// Получение ролей пользователя.
        /// </summary>
        /// <response code="200">Успешный возврат ролей. </response>
        /// <response code="500">Ошибка сервера. </response>
        [Produces("application/json")]
        [ProducesResponseType(typeof(RoleResponse), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        [HttpGet("get")]
        public async Task<IActionResult> GetRoles()
        {
            var roles = await _roleService.GetRolesAsync();
            return Ok(roles);
        }
    }
}
