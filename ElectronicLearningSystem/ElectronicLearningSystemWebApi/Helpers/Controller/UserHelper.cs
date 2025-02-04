using AutoMapper;
using ElectronicLearningSystemWebApi.Enums;
using ElectronicLearningSystemWebApi.Helpers.Exceptions;
using ElectronicLearningSystemWebApi.Models.UserModel;
using ElectronicLearningSystemWebApi.Models.UserModel.Response;
using ElectronicLearningSystemWebApi.Repositories.User;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace ElectronicLearningSystemWebApi.Helpers.Controller
{
    /// <summary>
    /// Хелпер для работы с пользователями.
    /// </summary>
    /// <param name="userRepository">Репозиторий для работы с пользователями. </param>
    /// <param name="httpContextAccessor">Контекст выполнения запроса. </param>
    /// <param name="mapper">Маппер. </param>
    public class UserHelper(IUserRepository userRepository,
        IHttpContextAccessor httpContextAccessor,
        IMapper mapper)
    {
        /// <summary>
        /// Репозиторий для работы с пользователями.
        /// </summary>
        protected readonly IUserRepository _userRepository = userRepository
            ?? throw new ArgumentNullException(nameof(userRepository));

        /// <summary>
        /// Контекст выполнения запроса.
        /// </summary>
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor
            ?? throw new ArgumentNullException(nameof(httpContextAccessor));

        /// <summary>
        /// Маппер.
        /// </summary>
        private readonly IMapper _mapper = mapper 
            ?? throw new ArgumentNullException(nameof(mapper));

        /// <summary>
        /// Получение идентификатора текущего пользователя.
        /// </summary>
        /// <returns></returns>
        public virtual Guid GetCurrentUserId()
        {
            var userNameIdentifier = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            ArgumentException.ThrowIfNullOrWhiteSpace(userNameIdentifier, nameof(userNameIdentifier));

            if (string.IsNullOrEmpty(userNameIdentifier) || !Guid.TryParse(userNameIdentifier, out var userId))
                throw new ArgumentNullException(nameof(userId), "User ID is missing or invalid.");

            return userId;
        }

        /// <summary>
        /// Получение пользователя по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор пользователя. </param>
        public async Task<UserResponse> GetUserByIdAsync(Guid id)
        {
            var user = await _userRepository.GetRecordByIdAsync(id) 
                ?? throw new ArgumentNullException($"The user with id: {id} was not found");

            return _mapper.Map<UserResponse>(user);
        }

        /// <summary>
        /// Получение текущего авторизированного пользователя.
        /// </summary>
        /// <exception cref="ArgumentNullException">Пользователь не найден. </exception>
        public async Task<UserResponse> GetCurrentUserAsync()
        {
            var userId = GetCurrentUserId();
            var user = await _userRepository.GetRecordByIdAsync(userId) 
                ?? throw new ArgumentNullException($"The user with id: {userId} was not found");

            return _mapper.Map<UserResponse>(user);
        }

        /// <summary>
        /// Получение пользователей.
        /// </summary>
        public async Task<IList<UserResponse>> GetUsersAsync()
        {
            var users = await _userRepository.GetAllRecordsAsync();
            return _mapper.Map<IList<UserResponse>>(users);
        }

        /// <summary>
        /// Создание пользователя.
        /// </summary>
        /// <param name="userResponse">Данные для создания пользователя. </param>
        /// <exception cref="DublicateUserException">Найден дубликат пользователя. </exception>
        public async Task CreateUserAsync(CreateUserDTO userResponse)
        {
            var newUser = _mapper.Map<UserEntity>(userResponse);

            if (_userRepository.GetUserByLoginAsync(newUser.Email) != null)
                throw new DublicateUserException($"Duplicate user found {newUser.Email}");

            await _userRepository.AddRecordAsync(newUser);
        }
    }
}
