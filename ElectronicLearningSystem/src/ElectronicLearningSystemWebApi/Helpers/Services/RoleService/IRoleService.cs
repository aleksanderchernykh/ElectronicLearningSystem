using ElectronicLearningSystemWebApi.Models.RoleModel.Response;

namespace ElectronicLearningSystemWebApi.Helpers.Services.RoleService
{
    /// <summary>
    /// Интерфейс сервиса для работы с ролями.
    /// </summary>
    public interface IRoleService
    {
        /// <summary>
        /// Получение всех ролей.
        /// </summary>
        /// <returns>Список ролей.</returns>
        Task<IList<RoleResponse>> GetRolesAsync();
    }
}
