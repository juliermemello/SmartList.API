using Ardalis.GuardClauses.Net9;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using SmartList.Domain.Common;
using SmartList.Domain.Interfaces.Repositories;
using SmartList.Infrastructure.Context;
using System.Linq.Expressions;

namespace SmartList.Infrastructure.Repositories;

public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
{
    protected readonly AppDbContext _context;
    protected readonly DbSet<T> _dbSet;
    protected IMapper _mapper;

    public BaseRepository(AppDbContext context, IMapper mapper)
    {
        _context = Guard.Against.Null(context, nameof(context));
        _mapper = Guard.Against.Null(mapper, nameof(mapper));

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
            entity.CreatedAt = exist.CreatedAt;
            entity.UpdatedAt = DateTime.Now;

            _context.Entry(exist).CurrentValues.SetValues(entity);
        }

        return Task.CompletedTask;
    }

    public virtual Task DeleteAsync(T entity)
    {
        T? exist = _dbSet.Find(entity.Id);

        if (exist is not null)
        {
            entity.Deleted = true;
            entity.DeletedAt = DateTime.Now;

            _context.Entry(exist).CurrentValues.SetValues(entity);
        }

        return Task.CompletedTask;
    }

    public virtual async Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? predicate = null)
    {
        var query = _dbSet
            .AsNoTracking()
            .ProjectTo<T>(_mapper.ConfigurationProvider);

        if (predicate is not null)
            query = query.Where(predicate);

        return await query
            .ToListAsync();
    }

    public virtual async Task<T?> GetByIdAsync(int id)
    {
        return await _dbSet
            .AsNoTracking()
            .ProjectTo<T>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync<T>(x => x.Id == id);
    }

    public virtual async Task<(IEnumerable<T> data, int total)> GetPagedAsync(
        Expression<Func<T, bool>>? predicate = null,
        Expression<Func<T, object>>? orderBy = null,
        int pageNumber = 1,
        int pageSize = 10)
    {
        var query = _dbSet
            .AsNoTracking()
            .ProjectTo<T>(_mapper.ConfigurationProvider);

        if (predicate is not null)
            query = query.Where(predicate);

        int total = await query.CountAsync();

        if (orderBy is not null)
            query = query.OrderBy(orderBy);
        else
            query = query.OrderByDescending(x => x.Id);

        var data = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (data, total);
    }
}
