using AutoMapper;
using ElectronicLearningSystemWebApi.Enums;
using ElectronicLearningSystemWebApi.Helpers.Exceptions;
using ElectronicLearningSystemWebApi.Models.UserModel;
using ElectronicLearningSystemWebApi.Models.UserModel.Response;
using ElectronicLearningSystemWebApi.Repositories.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ElectronicLearningSystemWebApi.Controllers
{
    /// <summary>
    /// Контроллер для работы с пользователем.
    /// </summary>
    /// <param name="userRepository">Репозиторий для работы с пользователем.</param>
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(IUserRepository userRepository,
        ILogger<UserController> logger,
        IMapper mapper) : ControllerBase
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<UserController> _logger = logger;

        /// <summary>
        /// Создание нового пользователя.
        /// </summary>
        /// <param name="userResponse"></param>
        /// <returns></returns>
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreateUserDTO userResponse)
        {
            try
            {
                var newUser = _mapper.Map<UserEntity>(userResponse);
                if (newUser is null)
                {
                    _logger.LogError(new EventId((int)EventLoggerEnum.InvalidMapEntity), message: $"Invalid map user {userResponse.Login}");
                    return BadRequest();
                }

                await _userRepository.AddRecordAsync(newUser);
                return StatusCode(201);
            }
            catch (DublicateUserException ex)
            {
                _logger.LogError(new EventId((int)EventLoggerEnum.DublicateUserException), message: $"Duplicate user was found {userResponse.Email}");
                return StatusCode(409, new { ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(new EventId((int)EventLoggerEnum.DataBaseException), message: ex.ToString());
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Получение пользователей.
        /// </summary>
        /// <returns></returns>
        [HttpGet("getusers")]
        public async Task<IActionResult> GetUsers()
        {
            try
            {
                var users = await _userRepository.GetAllRecordAsync();
                return Ok(users);
            }
            catch (Exception ex)
            {
                _logger.LogError(new EventId((int)EventLoggerEnum.DataBaseException), message: ex.ToString());
                return BadRequest(500);
            }
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

            try
            {
                var user = await _userRepository.GetUserByLoginAsync(User.Identity.Name);
               
                if (user == null)
                {
                    return Unauthorized();
                }
                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(new EventId((int)EventLoggerEnum.DataBaseException), message: ex.ToString());
                return BadRequest(500);
            }
        }

        /// <summary>
        /// Получение пользователя по id.
        /// </summary>
        /// <param name="id">Идентификатор.</param>
        /// <returns>Пользователь.</returns>
        [HttpGet("getuser/{id}")]
        public async Task<IActionResult> GetUserByIdAsync(Guid id)
        {
            try
            {
                var user = await _userRepository.GetRecordByIdAsync(id);

                if (user == null)
                {
                    return NotFound();
                }

                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError((int)EventLoggerEnum.DataBaseException, message: ex.ToString());
                return BadRequest(500);
            }
        }
    }
}
