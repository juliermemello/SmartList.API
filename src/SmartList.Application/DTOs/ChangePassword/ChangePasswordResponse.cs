namespace SmartList.Application.DTOs.ChangePassword;

public record ChangePasswordResponse
{
    public string Username { get; init; } = string.Empty;
    public bool PasswordChanged { get; init; } = true;
}
