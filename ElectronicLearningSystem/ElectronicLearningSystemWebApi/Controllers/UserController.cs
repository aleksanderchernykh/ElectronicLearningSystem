using AutoMapper;
using ElectronicLearningSystemWebApi.Attributes;
using ElectronicLearningSystemWebApi.Enums;
using ElectronicLearningSystemWebApi.Helpers.Controller;
using ElectronicLearningSystemWebApi.Helpers.Exceptions;
using ElectronicLearningSystemWebApi.Models.ErrorModel;
using ElectronicLearningSystemWebApi.Models.RoleModel.Response;
using ElectronicLearningSystemWebApi.Models.UserModel.Response;
using ElectronicLearningSystemWebApi.Repositories.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ElectronicLearningSystemWebApi.Controllers
{
    /// <summary>
    /// Контроллер для работы с пользователями.
    /// </summary>
    /// <param name="userHelper">Хелпер для работы с пользователями. </param>
    [Authorize]
    [Route("user")]
    [ApiController]
    public class UserController(UserHelper userHelper) : ControllerBase
    {
        /// <summary>
        /// Хелпер для работы с пользователями.
        /// </summary>
        private readonly UserHelper _userHelper = userHelper 
            ?? throw new ArgumentNullException(nameof(userHelper));

        /// <summary>
        /// Создание пользователя.
        /// </summary>
        /// <param name="userDTO">Данные для создания пользователя. </param>
        /// <response code="201">Успешное создание пользователя. </response>
        /// <response code="409">Успешное создание пользователя. </response>
        /// <response code="500">Ошибка сервера. </response>
        [Produces("application/json")]
        [ValidateModel]
        [ProducesResponseType(typeof(UserResponse), 201)]
        [ProducesResponseType(typeof(ErrorResponse), 409)]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreateUserDTO userDTO)
        {
            await _userHelper.CreateUserAsync(userDTO);
            return Created();
        }

        /// <summary>
        /// Получение пользователей.
        /// </summary>
        /// <response code="200">Успешный возврат всех пользователей. </response>
        /// <response code="500">Ошибка сервера. </response>
        [Produces("application/json")]
        [ProducesResponseType(typeof(UserResponse), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        [HttpGet("get")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userHelper.GetUsersAsync();
            return Ok(users);
        }

        /// <summary>
        /// Получение текущего профиля пользователя.
        /// </summary>
        /// <response code="200">Успешный возврат пользователя. </response>
        /// <response code="500">Ошибка сервера. </response>
        [Produces("application/json")]
        [ProducesResponseType(typeof(UserResponse), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        [HttpGet("getme")]
        public async Task<IActionResult> GetMe()
        {
            var user = await _userHelper.GetCurrentUserAsync();
            return Ok(user);
        }

        /// <summary>
        /// Получение пользователя по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор пользователя. </param>
        /// <response code="200">Успешный возврат пользователя по идентификатору. </response>
        /// <response code="500">Ошибка сервера. </response>
        [Produces("application/json")]
        [ProducesResponseType(typeof(UserResponse), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetUserByIdAsync(Guid id)
        {
            var user = await _userHelper.GetUserByIdAsync(id);
            return Ok(user);
        }
    }
}
