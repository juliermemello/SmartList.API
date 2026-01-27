namespace SmartList.Domain.Models;

public class ErrorResponse
{
    public int Status { get; init; }
    public string Message { get; init; } = string.Empty;
    public string? Details { get; init; }
    public Dictionary<string, string[]> Errors { get; init; } = new();
}
