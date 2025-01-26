using AutoMapper;
using ElectronicLearningSystemKafka.Common.Models;
using ElectronicLearningSystemWebApi.Models.CommentModel;
using ElectronicLearningSystemWebApi.Models.CommentModel.DTO;
using ElectronicLearningSystemWebApi.Models.EmailModel.DTO;
using ElectronicLearningSystemWebApi.Models.UserModel;
using ElectronicLearningSystemWebApi.Models.UserModel.Response;
using ElectronicLearningSystemWebApi.Repositories.TaskRepository;

namespace ElectronicLearningSystemWebApi.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserEntity, UserDto>()
                .ForSourceMember(src => src.Role, opt => opt.DoNotValidate())
                .ForSourceMember(src => src.Password, opt => opt.DoNotValidate())
                .ForSourceMember(src => src.RefreshTokenExpiryTime, opt => opt.DoNotValidate())
                .ForSourceMember(src => src.RefreshToken, opt => opt.DoNotValidate());
        }
    }
}