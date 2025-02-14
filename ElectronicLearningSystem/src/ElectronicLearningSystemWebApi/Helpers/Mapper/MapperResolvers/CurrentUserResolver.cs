using AutoMapper;
using ElectronicLearningSystemWebApi.Models;
using ElectronicLearningSystemWebApi.Services.UserService;

namespace ElectronicLearningSystemWebApi.Helpers.Mapper.MapperResolvers
{
    public class CurrentUserResolver(UserService userHelper)
        : IValueResolver<object, EntityBase, Guid?>
    {
        protected readonly UserService _userHelper = userHelper
            ?? throw new ArgumentNullException(nameof(_userHelper));

        public Guid? Resolve(object source, EntityBase destination, Guid? destMember, ResolutionContext context)
        {
            return _userHelper.GetCurrentUserId();
        }
    }
}
