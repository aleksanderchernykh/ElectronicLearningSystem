using AutoMapper;
using ElectronicLearningSystemWebApi.Helpers.Mapper.MapperResolvers;
using ElectronicLearningSystemWebApi.Models;
using ElectronicLearningSystemWebApi.Models.CommentModel.DTO;
using ElectronicLearningSystemWebApi.Models.CommentModel.Entity;
using ElectronicLearningSystemWebApi.Models.CommentModel.Response;
using ElectronicLearningSystemWebApi.Models.NotificationModel.DTO;
using ElectronicLearningSystemWebApi.Models.NotificationModel.Entity;
using ElectronicLearningSystemWebApi.Models.NotificationModel.Response;
using ElectronicLearningSystemWebApi.Models.RoleModel.Entity;
using ElectronicLearningSystemWebApi.Models.RoleModel.Response;
using ElectronicLearningSystemWebApi.Models.TaskModel.Entity;
using ElectronicLearningSystemWebApi.Models.TaskModel.Response;
using ElectronicLearningSystemWebApi.Models.UserModel.Entity;
using ElectronicLearningSystemWebApi.Models.UserModel.Response;

namespace ElectronicLearningSystemWebApi.Helpers.Mapper
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

            // Маппинг базовых объектов при возвращении.
            CreateMap<EntityBase, BaseResponse>()
                .ForSourceMember(src => src.CreatedBy, opt => opt.DoNotValidate())
                .ForSourceMember(src => src.ModifiedBy, opt => opt.DoNotValidate());

            // Маппинг возвращаемых значений "Пользователь".
            CreateMap<UserEntity, UserResponse>()
                .ForSourceMember(src => src.Role, opt => opt.DoNotValidate())
                .ForSourceMember(src => src.Password, opt => opt.DoNotValidate())
                .ForSourceMember(src => src.RefreshTokenExpiryTime, opt => opt.DoNotValidate())
                .ForSourceMember(src => src.RefreshToken, opt => opt.DoNotValidate());

            // Маппинг возвращаемых значений "Комментарий".
            CreateMap<CommentEntity, CommentResponse>()
                .IncludeBase<EntityBase, BaseResponse>()
                .ForSourceMember(src => src.Task, opt => opt.DoNotValidate());

            // Маппинг значений при создании записи "Комментарий".
            CreateMap<CreateCommentDTO, CommentEntity>()
                .IncludeBase<object, EntityBase>();

            // Маппинг значений при создании записи "Уведомление".
            CreateMap<CreateNotificationDTO, NotificationEntity>()
                .IncludeBase<object, EntityBase>()
                .ForMember(dest => dest.IsReady, opt => opt.MapFrom(_ => false));

            // Маппинг возвращаемых значений "Уведомление".
            CreateMap<NotificationEntity, NotificationResponse>()
                .IncludeBase<EntityBase, BaseResponse>()
                .ForSourceMember(src => src.Recipient, opt => opt.DoNotValidate())
                .ForSourceMember(src => src.NotificationType, opt => opt.DoNotValidate());

            // Маппинг возвращаемых значений "Роль".
            CreateMap<RoleEntity, RoleResponse>()
                .IncludeBase<EntityBase, BaseResponse>();

            // Маппинг возвращаемых значений "Задание".
            CreateMap<TaskEntity, TaskRespose>()
                .IncludeBase<EntityBase, BaseResponse>()
                .ForSourceMember(src => src.Student, opt => opt.DoNotValidate())
                .ForSourceMember(src => src.Owner, opt => opt.DoNotValidate());
        }
    }
}