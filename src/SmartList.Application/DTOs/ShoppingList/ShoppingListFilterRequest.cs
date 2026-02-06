namespace SmartList.Application.DTOs.ShoppingList;

public record ShoppingListFilterRequest
{
    public string? Name { get; init; } = null;
}
