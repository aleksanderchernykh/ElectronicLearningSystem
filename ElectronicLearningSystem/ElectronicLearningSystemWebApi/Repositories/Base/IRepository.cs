using ElectronicLearningSystemWebApi.Models;

namespace ElectronicLearningSystemWebApi.Repositories.Base
{
    /// <summary>
    /// Базовый интерфейс для работы с репозиториями.
    /// </summary>
    /// <typeparam name="T">Объект в БД от класса EntityBase.</typeparam>
    public interface IRepository<T>
        where T : EntityBase
    {
        Task<IList<T>> GetAllRecordAsync();

        Task<T?> GetRecordByIdAsync(Guid id);

        Task UpdateRecordAsync(T record);

        Task AddRecordAsync(T record);

        Task DeleteRecord(T record);
    }
}
