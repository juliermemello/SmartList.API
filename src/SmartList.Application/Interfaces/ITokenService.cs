using SmartList.Application.DTOs.User;

namespace SmartList.Application.Interfaces;

public interface ITokenService
{
    string GenerateToken(UserResponse user);
}
