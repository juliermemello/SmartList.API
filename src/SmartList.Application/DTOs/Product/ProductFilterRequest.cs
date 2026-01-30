namespace SmartList.Application.DTOs.Product;

public record ProductFilterRequest
{
    public string Name { get; init; } = string.Empty;
    public string DefaultEan { get; init; } = string.Empty;
    public int CategoryId { get; init; } = 0;
    public string Category { get; init; } = string.Empty;
}
