using Ardalis.GuardClauses.Net9;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartList.Application.DTOs.Login;
using SmartList.Application.Interfaces;
using SmartList.Domain.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace SmartList.API.Controllers;

[ApiVersion("1.0")]
public class AuthenticationController : BaseController
{
    private readonly IUserService _userService;

    public AuthenticationController(IUserService userService)
    {
        _userService = Guard.Against.Null(userService, nameof(userService));
    }

    [AllowAnonymous]
    [HttpPost]
    [SwaggerOperation(
        Summary = "Autentica um usuário e gera um token de acesso.",
        Description = "Realiza a autenticação do usuário utilizando credenciais (e-mail e senha). Se as credenciais forem válidas, o sistema retorna um token JWT para acesso a recursos protegidos e informações básicas do perfil.",
        OperationId = "AuthenticationLogin",
        Tags = ["Authentication"]
    )]
    [SwaggerResponse(200, "Usuário autenticado com sucesso.", typeof(LoginResponse))]
    [SwaggerResponse(400, "Não foi possível autenticar o usuário ou contém informações incorretas.", typeof(ErrorResponse))]
    [SwaggerResponse(401, "Não foi possível autenticar o usuário.", typeof(ErrorResponse))]
    public async Task<IActionResult> Login(
        [FromBody, SwaggerRequestBody("Dados para autenticação de um usuário.", Required = true)] LoginRequest request
    )
    {
        var result = await _userService.AuthenticateAsync(request);

        return CreatedAtAction(nameof(Login), new { id = result.Username }, result);
    }
}
