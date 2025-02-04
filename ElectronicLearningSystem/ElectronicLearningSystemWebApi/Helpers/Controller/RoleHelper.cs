
using AutoMapper;
using ElectronicLearningSystemWebApi.Models.RoleModel.Entity;
using ElectronicLearningSystemWebApi.Models.RoleModel.Response;
using ElectronicLearningSystemWebApi.Repositories.Base;

namespace ElectronicLearningSystemWebApi.Helpers.Controller
{
    public class RoleHelper(IRepository<RoleEntity> roleRepository, IMapper mapper)
    {
        private readonly IRepository<RoleEntity> _roleRepository = roleRepository
            ?? throw new ArgumentNullException(nameof(roleRepository));

        private readonly IMapper _mapper = mapper
            ?? throw new ArgumentNullException(nameof(mapper));

        public async Task<IList<RoleResponse>> GetRolesAsync()
        {
            var roles = await _roleRepository.GetAllRecordsAsync();
            return _mapper.Map<IList<RoleResponse>>(roles);
        }
    }
}
