using ElectronicLearningSystemWebApi.Models.NotificationModel;
using ElectronicLearningSystemWebApi.Models.TaskModel;
using ElectronicLearningSystemWebApi.Repositories.Base;

namespace ElectronicLearningSystemWebApi.Repositories.TaskRepository
{
    public interface ITaskRepository : IRepository<TaskEntity>
    {
        Task<IList<TaskEntity>> GetTaskByCurrentUserAsync();

        TaskEntity? GetRecordById(Guid id);
    }
}
