using SmartList.Domain.Common;

namespace SmartList.Domain.Interfaces.Repositories;

public interface IUnitOfWork : IDisposable
{
    IUserRepository Users { get; }
    ICategoryRepository Categories { get; }
    IProductRepository Products { get; }
    IShoppingListRepository ShoppingList { get; }

    IBaseRepository<T> Repository<T>() where T : BaseEntity;

    Task BeginTransactionAsync(CancellationToken cancellationToken);

    Task<bool> CommitAsync(CancellationToken cancellationToken);

    Task RollbackAsync(CancellationToken cancellationToken);
}
