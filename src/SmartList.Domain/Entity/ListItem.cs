using SmartList.Domain.Common;

namespace SmartList.Domain.Entity;

public class ListItem : BaseEntity
{
    public int ShoppingListId { get; set; }
    public ShoppingList? ShoppingList { get; set; } = null;
    public int ProductId { get; set; }
    public Product? Product { get; set; } = null;
    public string Ean { get; set; } = string.Empty;
    public decimal Quantity { get; set; } = 0;
    public decimal UnitPrice { get; set; } = 0;
    public bool IsChecked { get; set; } = false;
    public decimal TotalPrice => Quantity * UnitPrice;
}
