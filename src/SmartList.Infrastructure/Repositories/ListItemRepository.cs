using Ardalis.GuardClauses.Net9;
using SmartList.Domain.Entity;
using SmartList.Domain.Interfaces.Repositories;
using SmartList.Infrastructure.Context;

namespace SmartList.Infrastructure.Repositories;

public class ListItemRepository : BaseRepository<ListItem>, IListItemRepository
{
    public ListItemRepository(AppDbContext context) : base(context)
    {
        Guard.Against.Null(context, nameof(context));
    }
}
