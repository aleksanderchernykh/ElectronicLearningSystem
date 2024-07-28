using ElectronicLearningSystemWebApi.Helpers.CustomException;
using ElectronicLearningSystemWebApi.Models.UserModel.Response;
using ElectronicLearningSystemWebApi.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ElectronicLearningSystemWebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("user")]
    public class UserController(UserRepository userRepository) : ControllerBase
    {
        /// <summary>
        /// Репозиторий для работы с пользователем.
        /// </summary>
        private readonly UserRepository _userRepository = userRepository;

        /// <summary>
        /// Создание нового пользователя.
        /// </summary>
        /// <param name="userResponse"></param>
        /// <returns></returns>
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] UserRequest userResponse)
        {
            try
            {
                await _userRepository.CreateUser(userResponse);
                return StatusCode(201, new { Message = "Пользователь создан" });
            }
            catch (DublicateUserException ex)
            {
                return StatusCode(409, new { ex.Message });
            }
            catch
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Получение ролей пользователя.
        /// </summary>
        /// <returns></returns>
        [HttpGet("getroles")]
        public async Task<IActionResult> GetRoles()
        {
            return Ok(await _userRepository.GetRoles());
        }

        /// <summary>
        /// Получение пользователей.
        /// </summary>
        /// <returns></returns>
        [HttpGet("getusers")]
        public async Task<IActionResult> GetUsers()
        {
            return Ok(await _userRepository.GetUsers());
        }

        /// <summary>
        /// Получение текущего пользователя.
        /// </summary>
        /// <returns>Пользователь.</returns>
        [HttpGet("getme")]
        public async Task<IActionResult> GetMe()
        {
            if (string.IsNullOrEmpty(User?.Identity?.Name))
            {
                return Unauthorized();
            }

            return Ok(await _userRepository.GetUserByLoginAsync(User.Identity.Name));
        }

        /// <summary>
        /// Получение пользователя по id.
        /// </summary>
        /// <param name="id">Идентификатор.</param>
        /// <returns>Пользователь.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(Guid id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);

            if (user == null)
            {
                return BadRequest("Пользователь не найден.");
            }

            return Ok(user);
        }
    }
}
