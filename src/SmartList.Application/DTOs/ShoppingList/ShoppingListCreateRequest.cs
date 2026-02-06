namespace SmartList.Application.DTOs.ShoppingList;

public record ShoppingListCreateRequest
{
    public string Name { get; init; } = string.Empty;
}
