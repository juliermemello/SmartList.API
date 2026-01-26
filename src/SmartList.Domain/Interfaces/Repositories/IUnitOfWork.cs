namespace SmartList.Domain.Interfaces.Repositories;

public interface IUnitOfWork : IDisposable
{
    IUserRepository Users { get; }
    IProductRepository Products { get; }
    IShoppingListRepository ShoppingList { get; }

    Task BeginTransactionAsync();

    Task<bool> CommitAsync();

    Task RollbackAsync();
}
