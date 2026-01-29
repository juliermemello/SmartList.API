namespace SmartList.Application.DTOs.Category;

public record CategoryUpdateRequest
{
    public int Id { get; set; } = 0;
    public string Name { get; init; } = string.Empty;
    public string Icon { get; init; } = string.Empty;
}
