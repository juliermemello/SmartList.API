using SmartList.Domain.Common;

namespace SmartList.Domain.Entity;

public class ShoppingList : BaseEntity, BaseUserIdEntity
{
    public string Name { get; set; } = string.Empty;
    public int UserId { get; set; }
    public User? User { get; set; } = null;
    public List<ListItem> Items { get; set; } = new List<ListItem>();
}
