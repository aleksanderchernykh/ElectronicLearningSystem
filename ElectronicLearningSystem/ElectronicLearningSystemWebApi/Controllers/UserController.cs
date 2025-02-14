using ElectronicLearningSystemWebApi.Attributes;
using ElectronicLearningSystemWebApi.Helpers.Services.UserService;
using ElectronicLearningSystemWebApi.Models.ErrorModel;
using ElectronicLearningSystemWebApi.Models.UserModel.DTO;
using ElectronicLearningSystemWebApi.Models.UserModel.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ElectronicLearningSystemWebApi.Controllers
{
    /// <summary>
    /// Контроллер для работы с пользователями.
    /// </summary>
    /// <param name="userService">Хелпер для работы с пользователями. </param>
    [Authorize]
    [Route("user")]
    [ApiController]
    public class UserController(IUserService userService) : ControllerBase
    {
        /// <summary>
        /// Хелпер для работы с пользователями.
        /// </summary>
        private readonly IUserService _userService = userService 
            ?? throw new ArgumentNullException(nameof(userService));

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
            await _userService.CreateUserAsync(userDTO);
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
            var users = await _userService.GetUsersAsync();
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
            var user = await _userService.GetCurrentUserAsync();
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
        [HttpGet("get/{id:guid}")]
        public async Task<IActionResult> GetUserByIdAsync(Guid id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            return Ok(user);
        }
    }
}
