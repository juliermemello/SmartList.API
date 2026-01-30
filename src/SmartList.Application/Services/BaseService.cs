using Ardalis.GuardClauses.Net9;
using AutoMapper;
using FluentValidation;
using SmartList.Application.Interfaces;
using SmartList.Domain.Common;
using SmartList.Domain.Interfaces.Repositories;
using System.Linq.Expressions;

namespace SmartList.Application.Services;

public class BaseService<TEntity, TRequest, TUpdateRequest, TResponse> : IBaseService<TEntity, TRequest, TUpdateRequest, TResponse>
    where TEntity : BaseEntity
    where TRequest : class
    where TUpdateRequest : class
    where TResponse : class
{
    protected readonly IUnitOfWork _uow;
    protected readonly IMapper _mapper;
    protected readonly IValidator<TRequest> _validator;
    protected readonly IValidator<TUpdateRequest> _updateValidator;

    protected BaseService(
        IUnitOfWork uow,
        IMapper mapper,
        IValidator<TRequest> validator,
        IValidator<TUpdateRequest> updateValidator)
    {
        _uow = Guard.Against.Null(uow, nameof(uow));
        _mapper = Guard.Against.Null(mapper, nameof(mapper));
        _validator = Guard.Against.Null(validator, nameof(validator));
        _updateValidator = Guard.Against.Null(updateValidator, nameof(updateValidator));
    }

    public virtual async Task<IEnumerable<TResponse>> GetAllAsync(Expression<Func<TEntity, bool>>? predicate = null)
    {
        var entities = await _uow.Repository<TEntity>().GetAllAsync(predicate);

        return _mapper.Map<IEnumerable<TResponse>>(entities);
    }

    public virtual async Task<TResponse?> GetByIdAsync(int id)
    {
        var entity = await _uow.Repository<TEntity>().GetByIdAsync(id);

        return _mapper.Map<TResponse>(entity);
    }

    public virtual async Task<TResponse> AddAsync(TRequest request)
    {
        var validationResult = await _validator.ValidateAsync(request);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var entity = _mapper.Map<TEntity>(request);

        await _uow.Repository<TEntity>().AddAsync(entity);

        await _uow.CommitAsync(default);

        return _mapper.Map<TResponse>(entity);
    }

    public virtual async Task<TResponse> UpdateAsync(int id, TUpdateRequest request)
    {
        var validationResult = await _updateValidator.ValidateAsync(request);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var entity = await _uow.Repository<TEntity>().GetByIdAsync(id);

        if (entity == null) 
            throw new Exception("Registro não encontrado.");

        _mapper.Map(request, entity);

        await _uow.Repository<TEntity>().UpdateAsync(entity);

        await _uow.CommitAsync(default);

        return _mapper.Map<TResponse>(entity);
    }

    public virtual async Task<TResponse> RemoveAsync(int id)
    {
        var entity = await _uow.Repository<TEntity>().GetByIdAsync(id);

        if (entity == null)
            throw new Exception("Registro não encontrado.");

        await _uow.Repository<TEntity>().DeleteAsync(entity);

        return _mapper.Map<TResponse>(entity);
    }
}
