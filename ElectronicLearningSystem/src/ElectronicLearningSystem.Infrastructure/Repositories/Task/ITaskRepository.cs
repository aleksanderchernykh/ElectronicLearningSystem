using ElectronicLearningSystem.Infrastructure.Models.TaskModel;
using ElectronicLearningSystem.Infrastructure.Repositories.Base;

namespace ElectronicLearningSystem.Infrastructure.Repositories.Task
{
    public interface ITaskRepository : IRepository<TaskEntity>
    {
        Task<IList<TaskEntity>> GetTaskByCurrentUserAsync();

        TaskEntity? GetRecordById(Guid id);
    }
}
