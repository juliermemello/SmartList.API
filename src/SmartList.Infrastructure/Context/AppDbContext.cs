using Ardalis.GuardClauses.Net9;
using Microsoft.EntityFrameworkCore;
using SmartList.Domain.Common;
using SmartList.Domain.Entity;
using SmartList.Domain.Interfaces;
using System.Linq.Expressions;
using System.Reflection;

namespace SmartList.Infrastructure.Context;

public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<ShoppingList> ShoppingLists { get; set; }
    public DbSet<ListItem> ListItems { get; set; }

    private readonly IUserSession _userSession;

    public AppDbContext(DbContextOptions<AppDbContext> options, IUserSession userSession) : base(options)
    {
        Guard.Against.Null(options, nameof(options));

        _userSession = Guard.Against.Null(userSession, nameof(userSession));
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        OnBeforeSaving();

        return base.SaveChangesAsync(cancellationToken);
    }

    public override int SaveChanges()
    {
        OnBeforeSaving();

        return base.SaveChanges();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        Guard.Against.Null(modelBuilder, nameof(modelBuilder));

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(modelBuilder);

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            var type = entityType.ClrType;
            var isSoftDelete = typeof(BaseEntity).IsAssignableFrom(type);
            var isUserOwned = typeof(BaseUserIdEntity).IsAssignableFrom(type);

            if (isSoftDelete || isUserOwned)
            {
                var parameter = Expression.Parameter(type, "e");

                Expression? combinedBody = null;

                if (isSoftDelete)
                {
                    var isDeletedProp = Expression.Property(parameter, nameof(BaseEntity.Deleted));

                    combinedBody = Expression.Equal(isDeletedProp, Expression.Constant(false));
                }

                if (isUserOwned && _userSession.UserId.HasValue)
                {
                    var userIdProp = Expression.Property(parameter, nameof(BaseUserIdEntity.UserId));
                    var userIdValue = Expression.Constant(_userSession.UserId, typeof(int?));
                    var userFilter = Expression.Equal(userIdProp, userIdValue);

                    combinedBody = combinedBody == null
                        ? userFilter
                        : Expression.AndAlso(combinedBody, userFilter);
                }

                if (combinedBody != null)
                {
                    var lambda = Expression.Lambda(combinedBody, parameter);

                    modelBuilder.Entity(type).HasQueryFilter(lambda);
                }
            }
        }
    }

    private void OnBeforeSaving()
    {
        var userId = _userSession.UserId;

        if (userId == null) 
            return;

        if (!ChangeTracker.Entries<BaseUserIdEntity>().Any())
            return;

        foreach (var entry in ChangeTracker.Entries<BaseUserIdEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    if (entry.Entity.UserId == 0)
                        entry.Entity.UserId = userId.Value;

                    break;
                case EntityState.Modified:
                    entry.Property(x => x.UserId).IsModified = false;
                    break;
            }
        }
    }
}
