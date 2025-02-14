using ElectronicLearningSystemWebApi.Models;
using System.Linq.Expressions;

namespace ElectronicLearningSystemWebApi.Repositories.Base
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

        Task UpdateRecordAsync(T record);

        Task<IList<T>> GetRecordsByQueryAsync(Expression<Func<T, bool>> predicate);

        Task<T?> GetFirstRecordsByQueryAsync(Expression<Func<T, bool>> predicate);

        Task AddRecordAsync(T record);

        Task DeleteRecord(T record);
    }
}
