using SmartList.Domain.Common;

namespace SmartList.Domain.Entity;

public class Category : BaseEntity, BaseUserIdEntity
{
    public string Name { get; set; } = string.Empty;
    public string Icon { get; set; } = "tag";
    public int UserId { get; set; }
    public User? User { get; set; } = null!;
    public List<Product> Products { get; set; } = new List<Product>();
}
