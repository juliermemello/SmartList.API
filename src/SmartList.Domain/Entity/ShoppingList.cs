using SmartList.Domain.Common;

namespace SmartList.Domain.Entity;

public class ShoppingList : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public int UserId { get; set; }
    public User User { get; set; } = new User();
    public List<ListItem> Items { get; set; } = new List<ListItem>();
}
