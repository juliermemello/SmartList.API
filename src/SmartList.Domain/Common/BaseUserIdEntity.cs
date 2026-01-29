using SmartList.Domain.Entity;

namespace SmartList.Domain.Common;

public interface BaseUserIdEntity
{
    public int UserId { get; set; }
    public User? User { get; set; }
}
