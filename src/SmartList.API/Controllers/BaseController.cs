using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace SmartList.API.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[Produces("application/json")]
[Consumes("application/json")]
public abstract class BaseController : ControllerBase
{
    protected int GetUserId()
    {
        var value = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        return int.Parse(value!);
    }

    protected string GetUserName()
    {
        var value = User.FindFirst(ClaimTypes.Name)?.Value;
        
        return value!;
    }

    protected string GetUserEmail()
    {
        var value = User.FindFirst(ClaimTypes.Email)?.Value;

        return value!;
    }

    protected string GetUserLogin()
    {
        var value = User.FindFirst(ClaimTypes.Upn)?.Value;

        return value!;
    }
}