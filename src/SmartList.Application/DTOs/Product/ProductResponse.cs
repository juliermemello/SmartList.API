namespace SmartList.Application.DTOs.Product;

public record ProductResponse
{
    public int Id { get; init; } = 0;
    public string Name { get; init; } = string.Empty;
    public string DefaultEan { get; init; } = string.Empty;
    public int CategoryId { get; init; } = 0;
    public string CategoryName { get; init; } = string.Empty;
    public DateTime? CreatedAt { get; init; } = null;
    public DateTime? UpdatedAt { get; init; } = null;
}
