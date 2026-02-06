namespace SmartList.Application.DTOs.ShoppingListItem;

public record ShoppingListItemResponse
{
    public int Id { get; init; } = 0;
    public int ShoppingListId { get; init; } = 0;
    public string ShoppingList { get; init; } = string.Empty;
    public int ProductId { get; init; } = 0;
    public string Product { get; init; } = string.Empty;
    public int CategoryId { get; init; } = 0;
    public string Category { get; init; } = string.Empty;
    public string Ean { get; init; } = string.Empty;
    public decimal Quantity { get; init; } = 0;
    public decimal UnitPrice { get; init; } = 0;
    public bool IsChecked { get; init; } = false;
    public decimal TotalPrice { get; init; } = 0;
    public DateTime? CreatedAt { get; init; } = null;
    public DateTime? UpdatedAt { get; init; } = null;
}
