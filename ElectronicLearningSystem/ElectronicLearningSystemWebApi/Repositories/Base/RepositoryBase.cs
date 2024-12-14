using ElectronicLearningSystemCore.Extensions;
using ElectronicLearningSystemWebApi.Context;
using ElectronicLearningSystemWebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace ElectronicLearningSystemWebApi.Repositories.Base
{
    /// <summary>
    /// Базовый репозиторий для работы с БД.
    /// </summary>
    /// <typeparam name="T">Объект в БД от класса EntityBase.</typeparam>
    public abstract class RepositoryBase<T> : IRepository<T>
        where T : EntityBase
    {
        /// <summary>
        /// Контекст подключения к БД.
        /// </summary>
        protected readonly ApplicationContext _context;

        /// <summary>
        /// Контекст определенной таблицы.
        /// </summary>
        protected readonly DbSet<T> _dbSet;

        /// <summary>
        /// ctor.
        /// </summary>
        /// <param name="context">Контекст подключения к БД.</param>
        public RepositoryBase(ApplicationContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _dbSet = _context.Set<T>();
        }

        /// <summary>
        /// Добавление новой записи.
        /// </summary>
        public async Task AddRecordAsync(T record)
        {
            ArgumentNullException.ThrowIfNull(record);

            await _dbSet.AddAsync(record);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Удаление записи.
        /// </summary>
        public async Task DeleteRecord(T record)
        {
            ArgumentNullException.ThrowIfNull(record);

            _dbSet.Remove(record);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Получение всех записей таблицы.
        /// </summary>
        public async Task<IList<T>> GetAllRecordAsync()
        {
            return await _dbSet.ToListAsync();
        }

        /// <summary>
        /// Получение записи таблицы по Id.
        /// </summary>
        public async Task<T?> GetRecordByIdAsync(Guid id)
        {
            id.ThrowIsDefault();

            return await _dbSet.FirstOrDefaultAsync(x => x.Id == id);
        }

        /// <summary>
        /// Обновление записи.
        /// </summary>
        public async Task UpdateRecordAsync(T record)
        {
            ArgumentNullException.ThrowIfNull(record);

            _dbSet.Update(record);
            await _context.SaveChangesAsync();
        }
    }
}
