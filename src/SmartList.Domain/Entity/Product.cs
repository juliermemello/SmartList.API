using SmartList.Domain.Common;

namespace SmartList.Domain.Entity;

public class Product : BaseEntity, BaseUserIdEntity
{
    public string Name { get; set; } = string.Empty;
    public string? DefaultEan { get; set; } = null;
    public int CategoryId { get; set; }
    public virtual Category? Category { get; set; } = null;
    public int UserId { get; set; }
    public virtual User? User { get; set; } = null;
}
