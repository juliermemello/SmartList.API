using Ardalis.GuardClauses.Net9;
using AutoMapper;
using SmartList.Domain.Entity;
using SmartList.Domain.Interfaces.Repositories;
using SmartList.Infrastructure.Context;

namespace SmartList.Infrastructure.Repositories;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    public UserRepository(AppDbContext context, IMapper mapper) : base(context, mapper)
    {
        Guard.Against.Null(context, nameof(context));
        Guard.Against.Null(mapper, nameof(mapper));
    }
}
