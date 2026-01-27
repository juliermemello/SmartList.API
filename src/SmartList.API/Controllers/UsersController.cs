using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartList.Application.DTOs.User;
using SmartList.Application.Interfaces;

namespace SmartList.API.Controllers;

[ApiVersion("1.0")]
public class UsersController : BaseController
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] UserCreateRequest request)
    {
        var result = await _userService.AddAsync(request);

        return CreatedAtAction(nameof(Register), new { id = result.Id }, result);
    }
}
