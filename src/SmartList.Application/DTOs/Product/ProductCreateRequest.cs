namespace SmartList.Application.DTOs.Product;

public record ProductCreateRequest
{
    public string Name { get; init; } = string.Empty;
    public string? DefaultEan { get; init; } = null;
    public int CategoryId { get; init; } = 0;
}
