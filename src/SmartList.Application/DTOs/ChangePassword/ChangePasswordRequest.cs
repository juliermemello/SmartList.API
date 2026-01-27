namespace SmartList.Application.DTOs.ChangePassword;

public record ChangePasswordRequest
{
    public string Username { get; init; } = string.Empty;
    public string OldPassword { get; init; } = string.Empty;
    public string NewPassword { get; init; } = string.Empty;
}
