namespace SmartList.Application.DTOs.ShoppingList;

public record ShoppingListResponse
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public int TotalProducts { get; init; }
    public int CompletedProducts { get; init; }
    public DateTime? CreatedAt { get; init; } = null;
    public DateTime? UpdatedAt { get; init; } = null;
}
