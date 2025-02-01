using AutoMapper;
using ElectronicLearningSystemWebApi.Models;

namespace ElectronicLearningSystemWebApi.Helpers.MapperResolvers
{
    public class CurrentUserResolver(UserHelper userHelper) 
        : IValueResolver<object, EntityBase, Guid?>
    {
        protected readonly UserHelper _userHelper = userHelper 
            ?? throw new ArgumentNullException(nameof(_userHelper));

        public Guid? Resolve(object source, EntityBase destination, Guid? destMember, ResolutionContext context)
        {
            return _userHelper.GetCurrentUserId();
        }
    }
}
