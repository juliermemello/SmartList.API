using Ardalis.GuardClauses.Net9;
using AutoMapper;
using FluentValidation;
using LinqKit;
using SmartList.Application.DTOs.ChangePassword;
using SmartList.Application.DTOs.Login;
using SmartList.Application.DTOs.User;
using SmartList.Application.Interfaces;
using SmartList.Domain.Constants;
using SmartList.Domain.Entity;
using SmartList.Domain.Enums;
using SmartList.Domain.Interfaces.Repositories;
using System.Linq.Expressions;

namespace SmartList.Application.Services;

public class UserService : BaseService<User, UserCreateRequest, UserUpdateRequest, UserResponse>, IUserService
{
    private readonly IPasswordHasher _passwordHasher;
    private readonly IValidator<LoginRequest> _loginValidator;
    private readonly ITokenService _tokenService;
    private readonly IValidator<ChangePasswordRequest> _changePasswordValidator;

    public UserService(
        IUnitOfWork uow,
        IMapper mapper,
        IValidator<UserCreateRequest> validator,
        IPasswordHasher passwordHasher,
        IValidator<LoginRequest> loginValidator,
        ITokenService tokenService,
        IValidator<ChangePasswordRequest> changePasswordValidator,
        IValidator<UserUpdateRequest> updateValidator)
        : base(uow, mapper, validator, updateValidator)
    {
        _passwordHasher = Guard.Against.Null(passwordHasher, nameof(passwordHasher));
        _loginValidator = Guard.Against.Null(loginValidator, nameof(loginValidator));
        _tokenService = Guard.Against.Null(tokenService, nameof(tokenService));
        _changePasswordValidator = Guard.Against.Null(changePasswordValidator, nameof(changePasswordValidator));
    }

    public override async Task<UserResponse> AddAsync(UserCreateRequest request)
    {
        var validationResult = await _validator.ValidateAsync(request);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var entity = _mapper.Map<User>(request);

        entity.Role = UserRole.User.ToString();
        entity.Password = _passwordHasher.Hash(request.Password);

        await _uow.Users.AddAsync(entity);

        var userSaved = await _uow.CommitAsync(default);

        if (!userSaved)
            throw new Exception("Um erro ocorreu ao tentar criar um novo usuário.");

        if (entity.Id > 0)
        {
            foreach (var categoryName in DefaultData.Categories)
            {
                var category = await _uow.Categories.AddAsync(new Category
                {
                    Name = categoryName,
                    UserId = entity.Id
                });

                await _uow.CommitAsync(default);

                if (!DefaultData.Catalog.TryGetValue(category.Name, out List<string>? products))
                    continue;

                foreach (var product in products)
                {
                    await _uow.Products.AddAsync(new Product
                    {
                        Name = product,
                        CategoryId = category.Id,
                        UserId = entity.Id
                    });

                    await _uow.CommitAsync(default);
                }
            }
        }

        return _mapper.Map<UserResponse>(entity);
    }

    public async Task<LoginResponse> AuthenticateAsync(LoginRequest request)
    {
        var validationResult = await _loginValidator.ValidateAsync(request);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var userEntity = _uow.Users.Entities.FirstOrDefault(u => u.Username == request.Username);

        if (userEntity is null)
            throw new UnauthorizedAccessException("Nome de usuário ou senha inválidos.");

        var validPassword = _passwordHasher.Verify(request.Password, userEntity.Password);

        if (!validPassword)
            throw new UnauthorizedAccessException("Nome de usuário ou senha inválidos.");

        var user = _mapper.Map<UserResponse>(userEntity);

        var token = _tokenService.GenerateToken(user);

        return new LoginResponse
        {
            Name = user.Name,
            Username = user.Username,
            Email = user.Email,
            Token = token
        };
    }

    public async Task<ChangePasswordResponse> ChangePasswordAsync(ChangePasswordRequest request)
    {
        var validationResult = await _changePasswordValidator.ValidateAsync(request);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var userEntity = _uow.Users.Entities.FirstOrDefault(u => u.Username == request.Username);

        if (userEntity is null)
            throw new UnauthorizedAccessException("Nome de usuário inválidos.");

        var validPassword = _passwordHasher.Verify(request.OldPassword, userEntity.Password);

        if (!validPassword)
            throw new UnauthorizedAccessException("Nome de usuário ou senha antiga inválidos.");

        userEntity.Password = _passwordHasher.Hash(request.NewPassword);

        await _uow.Users.UpdateAsync(userEntity);

        var success = await _uow.CommitAsync(default);

        return new ChangePasswordResponse
        { 
            Username = userEntity.Username,
            PasswordChanged = success
        };
    }

    public Expression<Func<User, bool>>? GetDynamicFilter(UserFilterRequest? request)
    {
        if (request == null)
            return null;

        var predicate = PredicateBuilder.New<User>(true);

        if (!string.IsNullOrEmpty(request.Name))
            predicate = predicate.And(p => p.Name.Contains(request.Name));

        if (!string.IsNullOrEmpty(request.Username))
            predicate = predicate.And(p => p.Username.Contains(request.Username));

        if (!string.IsNullOrEmpty(request.Email))
            predicate = predicate.And(p => p.Email.Contains(request.Email));

        return predicate;
    }
}
