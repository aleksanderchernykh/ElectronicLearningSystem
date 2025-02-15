using ElectronicLearningSystem.Infrastructure.Models;
using System.Linq.Expressions;

namespace ElectronicLearningSystem.Infrastructure.Repositories.Base
{
    /// <summary>
    /// Базовый интерфейс для работы с репозиториями.
    /// </summary>
    /// <typeparam name="T">Объект в БД от класса EntityBase.</typeparam>
    public interface IRepository<T>
        where T : EntityBase
    {
        Task<IList<T>> GetAllRecordsAsync();

        Task<T?> GetRecordByIdAsync(Guid id);

        System.Threading.Tasks.Task UpdateRecordAsync(T record);

        Task<IList<T>> GetRecordsByQueryAsync(Expression<Func<T, bool>> predicate);

        Task<T?> GetFirstRecordsByQueryAsync(Expression<Func<T, bool>> predicate);

        System.Threading.Tasks.Task AddRecordAsync(T record);

        System.Threading.Tasks.Task DeleteRecord(T record);
    }
}
