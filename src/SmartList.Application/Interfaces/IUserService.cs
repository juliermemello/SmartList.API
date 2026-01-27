using SmartList.Application.DTOs.User;
using SmartList.Domain.Entity;

namespace SmartList.Application.Interfaces;

public interface IUserService : IBaseService<User, UserCreateRequest, UserResponse>
{
}
