using AutoMapper;
using ElectronicLearningSystemWebApi.Helpers.MapperResolvers;
using ElectronicLearningSystemWebApi.Models;
using ElectronicLearningSystemWebApi.Models.CommentModel.DTO;
using ElectronicLearningSystemWebApi.Models.CommentModel.Entity;
using ElectronicLearningSystemWebApi.Models.CommentModel.Response;
using ElectronicLearningSystemWebApi.Models.UserModel;
using ElectronicLearningSystemWebApi.Models.UserModel.Response;

namespace ElectronicLearningSystemWebApi.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Маппинг базовых полей сущности при создании.
            CreateMap<object, EntityBase>()
                .ForMember(dest => dest.CreatedOn, opt => opt.MapFrom(_ => DateTime.Now))
                .ForMember(dest => dest.ModifiedOn, opt => opt.MapFrom(_ => DateTime.Now))
                .ForMember(dest => dest.CreatedById, opt => opt.MapFrom<CurrentUserResolver>())
                .ForMember(dest => dest.ModifiedById, opt => opt.MapFrom<CurrentUserResolver>());

            // Маппинг возвращаемых значений "Пользователь".
            CreateMap<UserEntity, UserResponse>()
                .ForSourceMember(src => src.Role, opt => opt.DoNotValidate())
                .ForSourceMember(src => src.Password, opt => opt.DoNotValidate())
                .ForSourceMember(src => src.RefreshTokenExpiryTime, opt => opt.DoNotValidate())
                .ForSourceMember(src => src.RefreshToken, opt => opt.DoNotValidate());

            // Маппинг возвращаемых значений "Комментарий".
            CreateMap<CommentEntity, CommentResponse>()
                .ForSourceMember(src => src.Task, opt => opt.DoNotValidate())
                .ForSourceMember(src => src.CreatedBy, opt => opt.DoNotValidate())
                .ForSourceMember(src => src.ModifiedBy, opt => opt.DoNotValidate());

            // Маппинг значений при создании записи "Комментарий".
            CreateMap<CreateCommentDTO, CommentEntity>()
                .IncludeBase<object, EntityBase>();
        }
    }
}