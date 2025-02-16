using ElectronicLearningSystem.Infrastructure.Context;
using ElectronicLearningSystem.Infrastructure.Models.TaskModel;
using ElectronicLearningSystem.Infrastructure.Repositories.Base;

namespace ElectronicLearningSystem.Infrastructure.Repositories.Task
{
    public class TaskRepository(ApplicationContext context)
        : RepositoryBase<TaskEntity>(context), ITaskRepository
    {
        public TaskEntity? GetRecordById(Guid id)
        {
            return _dbSet.Where(x => x.Id == id).FirstOrDefault();
        }

        public async Task<IList<TaskEntity>> GetTaskByCurrentUserAsync()
        {
            return await GetAllRecordsAsync();
        }
    }
}
