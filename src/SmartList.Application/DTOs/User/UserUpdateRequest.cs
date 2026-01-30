namespace SmartList.Application.DTOs.User;

public record UserUpdateRequest
{
    public string Name { get; init; } = string.Empty;
    public string Username { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
}
