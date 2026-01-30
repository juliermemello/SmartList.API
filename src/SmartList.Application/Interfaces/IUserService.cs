using SmartList.Application.DTOs.ChangePassword;
using SmartList.Application.DTOs.Login;
using SmartList.Application.DTOs.User;
using SmartList.Domain.Entity;
using System.Linq.Expressions;

namespace SmartList.Application.Interfaces;

public interface IUserService : IBaseService<User, UserCreateRequest, UserUpdateRequest, UserResponse>
{
    Task<LoginResponse> AuthenticateAsync(LoginRequest request);
    Task<ChangePasswordResponse> ChangePasswordAsync(ChangePasswordRequest request);
    Expression<Func<User, bool>>? GetDynamicFilter(UserFilterRequest? request);
}
