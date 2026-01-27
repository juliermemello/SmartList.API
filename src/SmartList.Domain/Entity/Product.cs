using SmartList.Domain.Common;

namespace SmartList.Domain.Entity;

public class Product : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string DefaultEan { get; set; } = string.Empty;
    public int UserId { get; set; }
    public User User { get; set; } = new User();
}
