using Ardalis.GuardClauses.Net9;
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
public class UsersController : BaseController
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = Guard.Against.Null(userService, nameof(userService));
    }

    [AllowAnonymous]
    [HttpPost("register")]
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

    [AllowAnonymous]
    [HttpPost("login")]
    [SwaggerOperation(
        Summary = "Autentica um usuário e gera um token de acesso.",
        Description = "Realiza a autenticação do usuário utilizando credenciais (e-mail e senha). Se as credenciais forem válidas, o sistema retorna um token JWT para acesso a recursos protegidos e informações básicas do perfil.",
        OperationId = "UsersLogin",
        Tags = ["Users"]
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

    [AllowAnonymous]
    [HttpPost("change-password")]
    [SwaggerOperation(
        Summary = "Altera a senha do usuário.",
        Description = "Permite que o usuário substitua sua senha atual por uma nova. O sistema valida a senha antiga antes de realizar a atualização para garantir a segurança.",
        OperationId = "UsersChangeLogin",
        Tags = ["Users"]
    )]
    [SwaggerResponse(200, "Usuário autenticado com sucesso.", typeof(ChangePasswordResponse))]
    [SwaggerResponse(400, "Não foi possível alterar a senha do usuário.", typeof(ErrorResponse))]
    [SwaggerResponse(401, "Não foi possível autenticar o usuário ou contém informações incorretas.", typeof(ErrorResponse))]
    public async Task<IActionResult> ChangePassword(
        [FromBody, SwaggerRequestBody("Dados para troca da senha de um usuário.", Required = true)] ChangePasswordRequest request
    )
    {
        var result = await _userService.ChangePasswordAsync(request);

        return CreatedAtAction(nameof(ChangePassword), result);
    }

    [HttpPut("update")]
    [SwaggerOperation(
        Summary = "Atualiza os dados de perfil do usuário.",
        Description = "Permite a modificação de informações cadastrais como nome e nome de usuário. O sistema valida se o novo username já está em uso antes de persistir as alterações.",
        OperationId = "UsersUpdate",
        Tags = ["Users"]
    )]
    [SwaggerResponse(200, "Usuário atualizado com sucesso.", typeof(UserResponse))]
    [SwaggerResponse(400, "Não foi possível alterar o usuário ou contém informações incorretas.", typeof(ErrorResponse))]
    public async Task<IActionResult> Update(
        [FromBody, SwaggerRequestBody("Dados para alteração de um novo usuário.", Required = true)] UserUpdateRequest request
    )
    {
        var result = await _userService.UpdateAsync(request);

        return CreatedAtAction(nameof(Update), new { id = result.Id }, result);
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("remove")]
    [SwaggerOperation(
        Summary = "Remove a conta do usuário.",
        Description = "Realiza a exclusão de uma conta do usuário. Dependendo da política de retenção, os dados podem ser anonimizados ou removidos permanentemente. Esta operação é irreversível.",
        OperationId = "UsersDelete",
        Tags = ["Users"]
    )]
    [SwaggerResponse(200, "Usuário excluído com sucesso.", typeof(UserResponse))]
    [SwaggerResponse(400, "Não foi possível excluir o usuário.", typeof(ErrorResponse))]
    public async Task<IActionResult> Remove(
        [FromQuery, SwaggerParameter("Id do usuário que será excluído.", Required = true)] int id
    )
    {
        var result = await _userService.RemoveAsync(id);

        return CreatedAtAction(nameof(Remove), new { id = result.Id }, result);
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("list")]
    [SwaggerOperation(
        Summary = "Lista todos os registros de usuários.",
        Description = "Lista todos os registros de usuários.",
        OperationId = "UsersGetAll",
        Tags = ["Users"]
    )]
    [SwaggerResponse(200, "Lista de usuários exibida com sucesso.", typeof(IEnumerable<UserResponse>))]
    [SwaggerResponse(400, "Não foi possível listar os usuários.", typeof(ErrorResponse))]
    public async Task<IActionResult> List(
        [FromQuery, SwaggerParameter("Filtros para refinar a listagem.", Required = false)] UserFilterRequest? request
    )
    {
        var filter = _userService.GetDynamicFilter(request);

        var result = await _userService.GetAllAsync(filter);

        return CreatedAtAction(nameof(List), result);
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("{id:int}")]
    [SwaggerOperation(
        Summary = "Obtém os detalhes de um usuário pelo ID.",
        Description = "Retorna as informações detalhadas de um usuário específico.",
        OperationId = "UsersGetById",
        Tags = ["Users"]
    )]
    [SwaggerResponse(200, "Usuário encontrado e retornado com sucesso.", typeof(UserResponse))]
    [SwaggerResponse(404, "Usuário não encontrado ou o ID fornecido é inválido.", typeof(ErrorResponse))]
    public async Task<IActionResult> GetById(
        [FromQuery, SwaggerParameter("ID único de identificação de um usuário.", Required = true)] int id
    )
    {
        var result = await _userService.GetByIdAsync(id);

        return CreatedAtAction(nameof(GetById), result);
    }
}
