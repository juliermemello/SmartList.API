using Ardalis.GuardClauses.Net9;
using Microsoft.EntityFrameworkCore;
using SmartList.Domain.Common;
using SmartList.Domain.Interfaces.Repositories;
using SmartList.Infrastructure.Context;

namespace SmartList.Infrastructure.Repositories;

public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
{
    protected readonly AppDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public BaseRepository(AppDbContext context)
    {
        _context = Guard.Against.Null(context, nameof(context));
        _dbSet = _context.Set<T>();
    }

    public virtual IQueryable<T> Entities => _dbSet;

    public virtual async Task<T> AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);

        return entity;
    }

    public virtual Task UpdateAsync(T entity)
    {
        T? exist = _dbSet.Find(entity.Id);

        if (exist is not null)
        {
            _context.Entry(exist).CurrentValues.SetValues(entity);
        }

        return Task.CompletedTask;
    }

    public virtual Task DeleteAsync(T entity)
    {
        _dbSet.Remove(entity);

        return Task.CompletedTask;
    }

    public virtual async Task<List<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public virtual async Task<T?> GetByIdAsync(int id)
    {
        return await _dbSet.FindAsync(id);
    }
}
