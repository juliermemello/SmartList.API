namespace SmartList.Application.DTOs.ShoppingList;

public record ShoppingListUpdateRequest
{
    public string Name { get; init; } = string.Empty;
}
