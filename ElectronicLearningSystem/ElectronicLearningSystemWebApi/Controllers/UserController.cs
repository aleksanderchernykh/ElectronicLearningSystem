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
    [Authorize]
    [Route("user")]
    [ApiController]
    public class UserController(IUserRepository userRepository,
        ILogger<UserController> logger,
        IMapper mapper) : ControllerBase
    {
        private readonly IUserRepository _userRepository = userRepository 
            ?? throw new ArgumentNullException(nameof(userRepository));

        private readonly ILogger<UserController> _logger = logger 
            ?? throw new ArgumentNullException(nameof(logger));

        private readonly IMapper _mapper = mapper 
            ?? throw new ArgumentNullException(nameof(mapper));

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreateUserDTO userResponse)
        {
            try
            {
                //var newUser = _mapper.Map<UserEntity>(userResponse);
                //if (newUser is null)
               // {
                   // _logger.LogError(new EventId((int)EventLoggerEnum.InvalidMapEntity), message: $"Invalid map user {userResponse.Login}");
                   // return BadRequest();
                //}

                //await _userRepository.AddRecordAsync(newUser);
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

        [HttpGet("getusers")]
        public async Task<IActionResult> GetUsers()
        {
            try
            {
                var users = await _userRepository.GetAllRecordsAsync();

                return Ok(_mapper.Map<IList<UserDto>>(users));
            }
            catch (Exception ex)
            {
                _logger.LogError(new EventId((int)EventLoggerEnum.DataBaseException), message: ex.ToString());
                return BadRequest(500);
            }
        }

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

                return Ok(_mapper.Map<UserDto>(user));
            }
            catch (Exception ex)
            {
                _logger.LogError((int)EventLoggerEnum.DataBaseException, message: ex.ToString());
                return BadRequest(500);
            }
        }
    }
}
