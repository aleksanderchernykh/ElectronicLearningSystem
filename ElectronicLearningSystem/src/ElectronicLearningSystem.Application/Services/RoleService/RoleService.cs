using AutoMapper;
using ElectronicLearningSystem.Application.Models.RoleModel.Response;
using ElectronicLearningSystem.Infrastructure.Models.RoleModel;
using ElectronicLearningSystem.Infrastructure.Repositories.Base;

namespace ElectronicLearningSystem.Application.Services.RoleService
{
    /// <summary>
    /// Хелпер для работы с ролями.
    /// </summary>
    /// <param name="roleRepository">Репозиторий для работы с ролями. </param>
    /// <param name="mapper">Маппер. </param>
    public class RoleService(IRepository<RoleEntity> roleRepository,
        IMapper mapper) : IRoleService
    {
        /// <summary>
        /// Репозиторий для работы с ролями.
        /// </summary>
        private readonly IRepository<RoleEntity> _roleRepository = roleRepository
            ?? throw new ArgumentNullException(nameof(roleRepository));

        /// <summary>
        /// Маппер. 
        /// </summary>
        private readonly IMapper _mapper = mapper
            ?? throw new ArgumentNullException(nameof(mapper));

        /// <summary>
        /// Получение всех ролей.
        /// </summary>
        public async Task<IList<RoleResponse>> GetRolesAsync()
        {
            var roles = await _roleRepository.GetAllRecordsAsync();
            return _mapper.Map<IList<RoleResponse>>(roles);
        }
    }
}
