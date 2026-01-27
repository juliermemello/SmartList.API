namespace SmartList.Application.DTOs.User;

public class UserResponse
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Username { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public bool Active { get; init; }
    public int TotalLists { get; init; }
    public int TotalProducts { get; init; }
}
