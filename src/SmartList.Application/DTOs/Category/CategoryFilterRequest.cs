namespace SmartList.Application.DTOs.Category;

public record CategoryFilterRequest
{
    public string Name { get; init; } = string.Empty;
}
