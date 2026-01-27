using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartList.Application.DTOs.ChangePassword;
using SmartList.Application.DTOs.Login;
using SmartList.Application.DTOs.User;
using SmartList.Application.Interfaces;
using SmartList.Domain.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace SmartList.API.Controllers;

[ApiVersion("1.0")]
[Authorize]
public class UsersController : BaseController
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("register")]
    [AllowAnonymous]
    [SwaggerOperation(
        Summary = "Cria um novo usuário no sistema com as informações fornecidas.",
        Description = "Cria um novo usuário no sistema. Este endpoint é público e não requer autenticação. Recebe os dados básicos do usuário, valida as informações enviadas e registra o usuário na base de dados. Retorna os detalhes do usuário criado ou mensagens de erro de validação caso os dados fornecidos sejam inválidos.",
        OperationId = "UsersRegister",
        Tags = ["Users"]
    )]
    [SwaggerResponse(201, "Usuário criado com sucesso.", typeof(UserResponse))]
    [SwaggerResponse(400, "Não foi possível criar o usuário ou contém informações incorretas.", typeof(ErrorResponse))]
    public async Task<IActionResult> Register(
        [FromBody, SwaggerRequestBody("Dados para criação de um novo usuário.", Required = true)] UserCreateRequest request
    )
    {
        var result = await _userService.AddAsync(request);

        return CreatedAtAction(nameof(Register), new { id = result.Id }, result);
    }

    [HttpPost("login")]
    [AllowAnonymous]
    [SwaggerOperation(
        Summary = "Autentica um usuário e gera um token de acesso.",
        Description = "Realiza a autenticação do usuário utilizando credenciais (e-mail e senha). Se as credenciais forem válidas, o sistema retorna um token JWT para acesso a recursos protegidos e informações básicas do perfil.",
        OperationId = "UsersLogin",
        Tags = ["Users"]
    )]
    [SwaggerResponse(201, "Usuário autenticado com sucesso.", typeof(LoginResponse))]
    [SwaggerResponse(400, "Não foi possível autenticar o usuário ou contém informações incorretas.", typeof(ErrorResponse))]
    [SwaggerResponse(401, "Não foi possível autenticar o usuário.", typeof(ErrorResponse))]
    public async Task<IActionResult> Login(
        [FromBody, SwaggerRequestBody("Dados para autenticação de um usuário.", Required = true)] LoginRequest request
    )
    {
        var result = await _userService.AuthenticateAsync(request);

        return CreatedAtAction(nameof(Register), new { id = result.Username }, result);
    }

    [HttpPost("change-password")]
    [AllowAnonymous]
    [SwaggerOperation(
        Summary = "Altera a senha do usuário.",
        Description = "Permite que o usuário substitua sua senha atual por uma nova. O sistema valida a senha antiga antes de realizar a atualização para garantir a segurança.",
        OperationId = "UsersChangeLogin",
        Tags = ["Users"]
    )]
    [SwaggerResponse(201, "Usuário autenticado com sucesso.", typeof(ChangePasswordResponse))]
    [SwaggerResponse(400, "Não foi possível alterar a senha do usuário.", typeof(ErrorResponse))]
    [SwaggerResponse(401, "Não foi possível autenticar o usuário ou contém informações incorretas.", typeof(ErrorResponse))]
    public async Task<IActionResult> ChangePassword(
        [FromBody, SwaggerRequestBody("Dados para troca da senha de um usuário.", Required = true)] ChangePasswordRequest request
    )
    {
        var result = await _userService.ChangePasswordAsync(request);

        return CreatedAtAction(nameof(Register), new { id = result.Username }, result);
    }

    [HttpPut("update")]
    [SwaggerOperation(
        Summary = "Atualiza os dados de perfil do usuário.",
        Description = "Permite a modificação de informações cadastrais como nome e nome de usuário. O sistema valida se o novo username já está em uso antes de persistir as alterações.",
        OperationId = "UsersUpdate",
        Tags = ["Users"]
    )]
    [SwaggerResponse(201, "Usuário atualizado com sucesso.", typeof(UserResponse))]
    [SwaggerResponse(400, "Não foi possível alterar o usuário ou contém informações incorretas.", typeof(ErrorResponse))]
    public async Task<IActionResult> Update(
        [FromBody, SwaggerRequestBody("Dados para alteração de um novo usuário.", Required = true)] UserUpdateRequest request
    )
    {
        var result = await _userService.UpdateAsync(request);

        return CreatedAtAction(nameof(Register), new { id = result.Id }, result);
    }

    [HttpGet("test")]
    public IActionResult Test()
    {
        return Ok(GetUserName());
    }
}
