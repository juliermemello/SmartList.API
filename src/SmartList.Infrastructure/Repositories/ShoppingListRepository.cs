using Ardalis.GuardClauses.Net9;
using AutoMapper;
using SmartList.Domain.Entity;
using SmartList.Domain.Interfaces.Repositories;
using SmartList.Infrastructure.Context;

namespace SmartList.Infrastructure.Repositories;

public class ShoppingListRepository : BaseRepository<ShoppingList>, IShoppingListRepository
{
    public ShoppingListRepository(AppDbContext context, IMapper mapper) : base(context, mapper)
    {
        Guard.Against.Null(context, nameof(context));
        Guard.Against.Null(mapper, nameof(mapper));
    }
}
