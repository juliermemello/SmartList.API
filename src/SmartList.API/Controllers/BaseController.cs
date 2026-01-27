using Microsoft.AspNetCore.Mvc;

namespace SmartList.API.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
public abstract class BaseController : ControllerBase
{
}