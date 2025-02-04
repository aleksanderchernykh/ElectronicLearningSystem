using ElectronicLearningSystemWebApi.Context;
using ElectronicLearningSystemWebApi.Helpers.Controller;
using ElectronicLearningSystemWebApi.Models.NotificationModel;
using ElectronicLearningSystemWebApi.Models.TaskModel.Entity;
using ElectronicLearningSystemWebApi.Repositories.Base;
using ElectronicLearningSystemWebApi.Repositories.Notification;

namespace ElectronicLearningSystemWebApi.Repositories.TaskRepository
{
    public class TaskRepository(ApplicationContext context, UserHelper userHelper)
        : RepositoryBase<TaskEntity>(context), ITaskRepository
    {
        protected readonly UserHelper _userHelper = userHelper 
            ?? throw new ArgumentNullException(nameof(userHelper));

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
