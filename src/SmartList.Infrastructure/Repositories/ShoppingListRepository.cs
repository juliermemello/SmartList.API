using Ardalis.GuardClauses.Net9;
using SmartList.Domain.Entity;
using SmartList.Domain.Interfaces.Repositories;
using SmartList.Infrastructure.Context;

namespace SmartList.Infrastructure.Repositories;

public class ShoppingListRepository : BaseRepository<ShoppingList>, IShoppingListRepository
{
    public ShoppingListRepository(AppDbContext context) : base(context)
    {
        Guard.Against.Null(context, nameof(context));
    }
}
