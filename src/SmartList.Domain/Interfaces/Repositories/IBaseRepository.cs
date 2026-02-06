using SmartList.Domain.Common;
using System.Linq.Expressions;

namespace SmartList.Domain.Interfaces.Repositories;

public interface IBaseRepository<T> where T : BaseEntity
{
    IQueryable<T> Entities { get; }

    Task<T?> GetByIdAsync(int id);

    Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? predicate = null);

    Task<T> AddAsync(T entity);

    Task UpdateAsync(T entity);

    Task DeleteAsync(T entity);

    Task<(IEnumerable<T> data, int total)> GetPagedAsync(
        Expression<Func<T, bool>>? predicate = null,
        Expression<Func<T, object>>? orderBy = null,
        int pageNumber = 1,
        int pageSize = 10);
}
