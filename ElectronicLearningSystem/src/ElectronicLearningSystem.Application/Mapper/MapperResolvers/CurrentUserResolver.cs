using AutoMapper;
using ElectronicLearningSystem.Core.Helpers;
using ElectronicLearningSystem.Infrastructure.Models;

namespace ElectronicLearningSystem.Application.Mapper.MapperResolvers
{
    public class CurrentUserResolver(IUserHelper userHelper)
        : IValueResolver<object, EntityBase, Guid?>
    {
        protected readonly IUserHelper _userHelper = userHelper
            ?? throw new ArgumentNullException(nameof(_userHelper));

        public Guid? Resolve(object source, EntityBase destination, Guid? destMember, ResolutionContext context)
        {
            return _userHelper.GetCurrentUserId();
        }
    }
}
