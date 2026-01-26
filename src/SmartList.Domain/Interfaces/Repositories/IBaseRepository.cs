using SmartList.Domain.Common;

namespace SmartList.Domain.Interfaces.Repositories;

public interface IBaseRepository<T> where T : BaseEntity
{
    IQueryable<T> Entities { get; }

    Task<T?> GetByIdAsync(int id);

    Task<List<T>> GetAllAsync();

    Task<T> AddAsync(T entity);

    Task UpdateAsync(T entity);

    Task DeleteAsync(T entity);
}
