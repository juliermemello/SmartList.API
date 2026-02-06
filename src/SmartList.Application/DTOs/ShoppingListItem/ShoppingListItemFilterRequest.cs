namespace SmartList.Application.DTOs.ShoppingListItem;

public record ShoppingListItemFilterRequest
{
    public int ProductId { get; init; } = 0;
    public string Product { get; init; } = string.Empty;
    public int CategoryId { get; init; } = 0;
    public string CategoryName { get; init; } = string.Empty;
    public string Ean { get; init; } = string.Empty;
}
