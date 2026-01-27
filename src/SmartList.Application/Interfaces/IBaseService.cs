using SmartList.Domain.Common;
using System.Linq.Expressions;

namespace SmartList.Application.Interfaces;

public interface IBaseService<TEntity, TRequest, TResponse>
    where TEntity : BaseEntity
    where TRequest : class
    where TResponse : class
{
    Task<IEnumerable<TResponse>> GetAllAsync(Expression<Func<TEntity, bool>>? predicate = null);

    Task<TResponse?> GetByIdAsync(int id);
    
    Task<TResponse> AddAsync(TRequest request);
    
    Task<TResponse> UpdateAsync(int id, TRequest request);
    
    Task<bool> RemoveAsync(int id);
}
