namespace SmartList.Application.DTOs.User;

public record UserFilterRequest
{
    public string Name { get; init; } = string.Empty;
    public string Username { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
}
