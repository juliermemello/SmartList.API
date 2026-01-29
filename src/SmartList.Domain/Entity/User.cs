using SmartList.Domain.Common;

namespace SmartList.Domain.Entity;

public class User : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public bool Active { get; set; } = true;
    public List<ShoppingList> Lists { get; set; } = new List<ShoppingList>();
    public List<Category> Categories { get; set; } = new List<Category>();
    public List<Product> Products { get; set; } = new List<Product>();
}
