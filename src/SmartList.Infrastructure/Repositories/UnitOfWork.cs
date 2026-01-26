using Ardalis.GuardClauses.Net9;
using Microsoft.EntityFrameworkCore.Storage;
using SmartList.Domain.Interfaces.Repositories;
using SmartList.Infrastructure.Context;

namespace SmartList.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    private IDbContextTransaction? _currentTransaction;

    public IUserRepository Users { get; }
    public IProductRepository Products { get; }
    public IShoppingListRepository ShoppingList { get; }
    public IListItemRepository ListItem { get; }

    public UnitOfWork(
        AppDbContext context,
        IUserRepository users,
        IProductRepository products,
        IShoppingListRepository shoppingList,
        IListItemRepository listItem)
    {
        _context = Guard.Against.Null(context, nameof(context));

        Users = Guard.Against.Null(users, nameof(users));
        Products = Guard.Against.Null(products, nameof(products));
        ShoppingList = Guard.Against.Null(shoppingList, nameof(shoppingList));
        ListItem = Guard.Against.Null(listItem, nameof(listItem));
    }

    public async Task BeginTransactionAsync()
    {
        _currentTransaction = await _context.Database.BeginTransactionAsync();
    }

    public async Task<bool> CommitAsync()
    {
        try
        {
            var success = await _context.SaveChangesAsync() > 0;

            if (_currentTransaction != null)
                await _currentTransaction.CommitAsync();

            return success;
        }
        catch
        {
            await RollbackAsync();

            throw;
        }
        finally
        {
            DisposeTransaction();
        }
    }

    public async Task RollbackAsync()
    {
        if (_currentTransaction != null)
            await _currentTransaction.RollbackAsync();

        DisposeTransaction();
    }

    private void DisposeTransaction()
    {
        _currentTransaction?.Dispose();
        _currentTransaction = null;
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
