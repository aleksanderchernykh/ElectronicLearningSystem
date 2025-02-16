using ElectronicLearningSystem.Application.Models;
using ElectronicLearningSystem.Application.Models.UserModel.DTO;
using ElectronicLearningSystem.Application.Models.UserModel.Response;

namespace ElectronicLearningSystem.Application.Services
{
    /// <summary>
    /// Интерфейс для работы с пользователями.
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Получение пользователя по идентификатору.
        /// </summary>
        Task<UserResponse> GetUserByIdAsync(Guid id);

        /// <summary>
        /// Получение анонимной записи пользователя по идентификатору.
        /// </summary>
        Task<BaseResponse> GetAnonymousUserByIdAsync(Guid id);

        /// <summary>
        /// Получение текущего авторизированного пользователя.
        /// </summary>
        Task<UserResponse> GetCurrentUserAsync();

        /// <summary>
        /// Получение всех пользователей.
        /// </summary>
        Task<IList<UserResponse>> GetUsersAsync();

        /// <summary>
        /// Создание нового пользователя.
        /// </summary>
        Task CreateUserAsync(CreateUserDTO userResponse);
    }
}
