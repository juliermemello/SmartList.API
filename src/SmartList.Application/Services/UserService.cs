using Ardalis.GuardClauses.Net9;
using AutoMapper;
using FluentValidation;
using SmartList.Application.DTOs.User;
using SmartList.Application.Interfaces;
using SmartList.Domain.Entity;
using SmartList.Domain.Interfaces.Repositories;

namespace SmartList.Application.Services;

public class UserService : BaseService<User, UserCreateRequest, UserResponse>, IUserService
{
    private readonly IPasswordHasher _passwordHasher;

    public UserService(
        IUnitOfWork uow,
        IMapper mapper,
        IValidator<UserCreateRequest> validator,
        IPasswordHasher passwordHasher)
        : base(uow, mapper, validator)
    {
        _passwordHasher = Guard.Against.Null(passwordHasher, nameof(passwordHasher));
    }

    public override async Task<UserResponse> AddAsync(UserCreateRequest request)
    {
        var validationResult = await _validator.ValidateAsync(request);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var entity = _mapper.Map<User>(request);

        entity.Password = _passwordHasher.Hash(request.Password);

        await _uow.Users.AddAsync(entity);
        await _uow.CommitAsync(default);

        return _mapper.Map<UserResponse>(entity);
    }
}
