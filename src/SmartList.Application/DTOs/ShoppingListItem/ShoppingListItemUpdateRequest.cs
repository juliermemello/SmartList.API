namespace SmartList.Application.DTOs.ShoppingListItem;

public record ShoppingListItemUpdateRequest
{
    public int ProductId { get; init; } = 0;
    public string Ean { get; init; } = string.Empty;
    public decimal Quantity { get; init; } = 0;
    public decimal UnitPrice { get; init; } = 0;
    public bool IsChecked { get; init; } = false;
}
