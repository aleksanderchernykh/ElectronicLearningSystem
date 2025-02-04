using ElectronicLearningSystemWebApi.Helpers.Controller;
using ElectronicLearningSystemWebApi.Models.ErrorModel;
using ElectronicLearningSystemWebApi.Models.RoleModel.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ElectronicLearningSystemWebApi.Controllers
{
    /// <summary>
    /// Контроллер для работы с ролями.
    /// </summary>
    /// <param name="roleHelper">Хелпер для работы с ролями. </param>
    [Authorize]
    [Route("role")]
    [ApiController]
    public class RoleController(RoleHelper roleHelper) 
        : ControllerBase
    {
        /// <summary>
        /// Хелпер для работы с ролями.
        /// </summary>
        private readonly RoleHelper _roleHelper = roleHelper 
            ?? throw new ArgumentNullException(nameof(roleHelper));

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
            var roles = await _roleHelper.GetRolesAsync();
            return Ok(roles);
        }
    }
}
