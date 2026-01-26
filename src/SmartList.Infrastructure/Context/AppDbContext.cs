using Ardalis.GuardClauses.Net9;
using Microsoft.EntityFrameworkCore;
using SmartList.Domain.Entity;
using System.Reflection;

namespace SmartList.Infrastructure.Context;

public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<ShoppingList> ShoppingLists { get; set; }
    public DbSet<ListItem> ListItems { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        Guard.Against.Null(options, nameof(options));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        Guard.Against.Null(modelBuilder, nameof(modelBuilder));

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(modelBuilder);
    }
}
