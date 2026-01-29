using Microsoft.AspNetCore.Http;
using SmartList.Domain.Interfaces;
using System.Security.Claims;

namespace SmartList.Infrastructure.Persistence;

public class UserSession : IUserSession
{
    private readonly IHttpContextAccessor _accessor;

    public UserSession(IHttpContextAccessor accessor)
    {
        _accessor = accessor;
    }

    public int? UserId
    {
        get
        {
            var claim = _accessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier);

            return claim != null ? int.Parse(claim.Value) : null;
        }
    }
}
