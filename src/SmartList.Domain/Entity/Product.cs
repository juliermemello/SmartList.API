using SmartList.Domain.Common;

namespace SmartList.Domain.Entity;

public class Product : BaseEntity, BaseUserIdEntity
{
    public string Name { get; set; } = string.Empty;
    public string? DefaultEan { get; set; } = null;
    public int CategoryId { get; set; }
    public Category? Category { get; set; } = null;
    public int UserId { get; set; }
    public User? User { get; set; } = null;
}
