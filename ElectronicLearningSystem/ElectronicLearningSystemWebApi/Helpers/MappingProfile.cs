using AutoMapper;
using ElectronicLearningSystemKafka.Common.Models;
using ElectronicLearningSystemWebApi.Helpers.Resolvers;
using ElectronicLearningSystemWebApi.Models.CommentModel;
using ElectronicLearningSystemWebApi.Models.CommentModel.DTO;
using ElectronicLearningSystemWebApi.Models.EmailModel.DTO;
using ElectronicLearningSystemWebApi.Repositories.TaskRepository;

namespace ElectronicLearningSystemWebApi.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<EmailSendingDTO, Email>();

           // CreateMap<CreateUserDTO, UserEntity>()
                //.ForMember(user => user.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
                //.ForMember(user => user.CreatedOn, opt => opt.MapFrom(src => DateTime.Now))
                //.ForMember(user => user.ModifiedOn, opt => opt.MapFrom(src => DateTime.Now));

            CreateMap<CreateCommentDTO, CommentEntity>()
                .ForMember(comment => comment.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
                .ForMember(comment => comment.Text, opt => opt.MapFrom(src => src.Text))
                .ForMember(comment => comment.TaskId, opt => opt.MapFrom(src => src.TaskId))
                .ForMember(comment => comment.Task, opt => opt.MapFrom<TaskResolver>())
                .ForMember(comment => comment.CreatedOn, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(comment => comment.ModifiedBy, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(comment => comment.ModifiedOn, opt => opt.MapFrom(src => DateTime.Now));
        }
    }
}