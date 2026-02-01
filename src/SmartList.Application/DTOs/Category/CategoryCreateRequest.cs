namespace SmartList.Application.DTOs.Category;

public record CategoryCreateRequest
{
    public string Name { get; init; } = string.Empty;
    public string? Icon { get; init; } = null;
}
