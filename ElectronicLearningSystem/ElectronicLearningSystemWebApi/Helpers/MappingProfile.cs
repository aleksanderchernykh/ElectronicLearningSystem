using AutoMapper;
using ElectronicLearningSystemKafka.Common.Models;
using ElectronicLearningSystemWebApi.Models.EmailModel.Response;
using ElectronicLearningSystemWebApi.Models.UserModel;
using ElectronicLearningSystemWebApi.Models.UserModel.Response;

namespace ElectronicLearningSystemWebApi.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<EmailSendingDTO, Email>();

            CreateMap<CreateUserDTO, UserEntity>()
                .ForMember(user => user.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
                .ForMember(user => user.CreatedOn, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(user => user.ModifiedOn, opt => opt.MapFrom(src => DateTime.Now));
        }
    }
}