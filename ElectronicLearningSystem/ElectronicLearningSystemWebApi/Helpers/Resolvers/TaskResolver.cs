using AutoMapper;
using ElectronicLearningSystemWebApi.Models.CommentModel.DTO;
using ElectronicLearningSystemWebApi.Models.CommentModel;
using ElectronicLearningSystemWebApi.Models.TaskModel;
using ElectronicLearningSystemWebApi.Repositories.TaskRepository;

namespace ElectronicLearningSystemWebApi.Helpers.Resolvers
{
    public class TaskResolver(ITaskRepository taskRepository)
        : IValueResolver<CreateCommentDTO, CommentEntity, TaskEntity>
    {
        private readonly ITaskRepository _taskRepository = taskRepository 
            ?? throw new ArgumentNullException(nameof(taskRepository));

        public TaskEntity Resolve(CreateCommentDTO source, CommentEntity destination, TaskEntity destMember, ResolutionContext context)
        {
            return _taskRepository.GetRecordById(source.TaskId) 
                ?? throw new ArgumentNullException($"Not search task by id {source.TaskId}");
        }
    }
}
