using AutoMapper;
using ElectronicLearningSystem.Application.Mapper.MapperResolvers;
using ElectronicLearningSystem.Application.Models;
using ElectronicLearningSystem.Application.Models.CommentModel.DTO;
using ElectronicLearningSystem.Application.Models.CommentModel.Response;
using ElectronicLearningSystem.Application.Models.NotificationModel.DTO;
using ElectronicLearningSystem.Application.Models.NotificationModel.Response;
using ElectronicLearningSystem.Application.Models.RoleModel.Response;
using ElectronicLearningSystem.Application.Models.TaskModel.Response;
using ElectronicLearningSystem.Application.Models.UserModel.Response;
using ElectronicLearningSystem.Infrastructure.Models;
using ElectronicLearningSystem.Infrastructure.Models.CommentModel;
using ElectronicLearningSystem.Infrastructure.Models.NotificationModel;
using ElectronicLearningSystem.Infrastructure.Models.RoleModel;
using ElectronicLearningSystem.Infrastructure.Models.TaskModel;
using ElectronicLearningSystem.Infrastructure.Models.UserModel;

namespace ElectronicLearningSystem.Application.Mapper
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
                .IncludeBase<EntityBase, BaseResponse>()
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