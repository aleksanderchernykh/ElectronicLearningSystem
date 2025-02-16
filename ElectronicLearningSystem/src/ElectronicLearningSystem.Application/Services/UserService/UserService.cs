using AutoMapper;
using ElectronicLearningSystem.Application.Models;
using ElectronicLearningSystem.Application.Models.UserModel.DTO;
using ElectronicLearningSystem.Application.Models.UserModel.Response;
using ElectronicLearningSystem.Core.CustomException;
using ElectronicLearningSystem.Core.Helpers;
using ElectronicLearningSystem.Infrastructure.Models;
using ElectronicLearningSystem.Infrastructure.Models.UserModel;
using ElectronicLearningSystem.Infrastructure.Repositories.User;

namespace ElectronicLearningSystem.Application.Services.UserService
{
    /// <summary>
    /// Хелпер для работы с пользователями.
    /// </summary>
    /// <param name="userRepository">Репозиторий для работы с пользователями. </param>
    /// <param name="userHelper">Хелпер для работы с пользователями. </param>
    /// <param name="mapper">Маппер. </param>
    public class UserService(IUserRepository userRepository,
        IUserHelper userHelper,
        IMapper mapper) : IUserService
    {
        /// <summary>
        /// Репозиторий для работы с пользователями.
        /// </summary>
        protected readonly IUserRepository _userRepository = userRepository
            ?? throw new ArgumentNullException(nameof(userRepository));

        /// <summary>
        /// Хелпер для работы с пользователями.
        /// </summary>
        protected readonly IUserHelper _userHelper = userHelper
            ?? throw new ArgumentNullException(nameof(userHelper));

        /// <summary>
        /// Маппер.
        /// </summary>
        private readonly IMapper _mapper = mapper
            ?? throw new ArgumentNullException(nameof(mapper));

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
            var userId = _userHelper.GetCurrentUserId();
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

        /// <summary>
        /// Получение анонимной записи пользователя по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор пользователя. </param>
        public async Task<BaseResponse> GetAnonymousUserByIdAsync(Guid id)
        {
            var user = await _userRepository.GetRecordByIdAsync(id)
                ?? throw new ArgumentNullException($"The user with id: {id} was not found");

            var anonymousUser = (EntityBase)user;
            return _mapper.Map<BaseResponse>(anonymousUser);
        }
    }
}
