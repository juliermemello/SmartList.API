using Ardalis.GuardClauses.Net9;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using SmartList.Application.DTOs.Product;
using SmartList.Application.DTOs.ShoppingList;
using SmartList.Application.Interfaces;
using SmartList.Domain.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace SmartList.API.Controllers;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/shopping-list")]
public class ShoppingListController : BaseController
{
    private readonly IShoppingListService _shoppingListService;

    public ShoppingListController(IShoppingListService shoppingListService)
    {
        _shoppingListService = Guard.Against.Null(shoppingListService, nameof(shoppingListService));
    }

    [HttpPost]
    [SwaggerOperation(
        Summary = "Cria uma nova lista de compras para o usuário.",
        Description = "Registra uma nova lista (ex: 'Compras do Mês' ou 'Churrasco') na conta do usuário autenticado. O sistema permite múltiplas listas, mas valida os dados obrigatórios para garantir a integridade.",
        OperationId = "ShoppingListsCreate",
        Tags = ["Shopping List"]
    )]
    [SwaggerResponse(201, "Lista de compras criada com sucesso.", typeof(ShoppingListResponse))]
    [SwaggerResponse(400, "Dados inválidos enviados na requisição.", typeof(ErrorResponse))]
    [SwaggerResponse(401, "Usuário não autenticado ou token inválido.", typeof(ErrorResponse))]
    public async Task<IActionResult> Create(
        [FromBody, SwaggerRequestBody("Dados para criação de uma nova lista de compras.", Required = true)] ShoppingListCreateRequest request
    )
    {
        var result = await _shoppingListService.AddAsync(request);

        return CreatedAtAction(nameof(Create), result);
    }

    [HttpPut("{id:int}")]
    [SwaggerOperation(
        Summary = "Atualiza os dados de uma lista de compras.",
        Description = "Modifica o nome ou as configurações de uma lista de compras específica do usuário autenticado. O sistema valida se a lista pertence ao usuário antes de aplicar as alterações.",
        OperationId = "ShoppingListsUpdate",
        Tags = ["Shopping List"]
    )]
    [SwaggerResponse(200, "Lista de compras atualizada com sucesso.", typeof(ShoppingListResponse))]
    [SwaggerResponse(400, "Dados inválidos enviados na requisição.", typeof(ErrorResponse))]
    [SwaggerResponse(404, "Lista de compras não encontrada ou não pertence ao usuário.", typeof(ErrorResponse))]
    public async Task<IActionResult> Update(
        [FromRoute, SwaggerParameter("Id da lista de compras que será atualizada.", Required = true)] int id,
        [FromBody, SwaggerRequestBody("Dados para atualização de uma lista de compras.", Required = true)] ShoppingListUpdateRequest request
    )
    {
        var result = await _shoppingListService.UpdateAsync(id, request);

        return Ok(result);
    }

    [HttpDelete("{id:int}")]
    [SwaggerOperation(
        Summary = "Remove uma lista de compras do usuário.",
        Description = "Realiza a exclusão lógica (Soft Delete) de uma lista de compras específica. A lista deixa de ser exibida nas consultas ativas, mas seus dados são preservados para fins de histórico e estatísticas. Operação restrita ao proprietário da lista.",
        OperationId = "ShoppingListsDelete",
        Tags = ["Shopping List"]
    )]
    [SwaggerResponse(200, "Lista de compras removida com sucesso.", typeof(ShoppingListResponse))]
    [SwaggerResponse(404, "Lista de compras não encontrada ou já removida.", typeof(ErrorResponse))]
    [SwaggerResponse(401, "Usuário não autenticado.", typeof(ErrorResponse))]
    public async Task<IActionResult> Remove(
        [FromRoute, SwaggerParameter("Id da lista de compras que será excluída.", Required = true)] int id
    )
    {
        var result = await _shoppingListService.RemoveAsync(id);

        return Ok(result);
    }

    [HttpGet]
    [SwaggerOperation(
        Summary = "Lista todas as listas de compras do usuário.",
        Description = "Recupera todas as listas de compras associadas ao usuário autenticado. O retorno inclui informações básicas de cada lista, como nome e data de criação, facilitando a navegação inicial.",
        OperationId = "ShoppingListsGetAll",
        Tags = ["Shopping List"]
    )]
    [SwaggerResponse(200, "Lista de compras recuperada com sucesso.", typeof(IEnumerable<ShoppingListResponse>))]
    [SwaggerResponse(400, "Erro ao processar a solicitação de listagem.", typeof(ErrorResponse))]
    public async Task<IActionResult> List(
        [FromQuery, SwaggerParameter("Filtros para refinar a listagem.", Required = false)] ShoppingListFilterRequest? request
    )
    {
        var filter = _shoppingListService.GetDynamicFilter(request);

        var result = await _shoppingListService.GetAllAsync(filter);

        return Ok(result);
    }

    [HttpGet("{id:int}")]
    [SwaggerOperation(
        Summary = "Obtém os detalhes de uma lista de compras pelo Id.",
        Description = "Retorna as informações completas de uma lista específica, incluindo a relação de todos os itens e produtos cadastrados nela. Esta consulta é restrita ao proprietário da lista.",
        OperationId = "ShoppingListsGetById",
        Tags = ["Shopping List"]
    )]
    [SwaggerResponse(200, "Lista de compras encontrada e retornada com sucesso.", typeof(ShoppingListResponse))]
    [SwaggerResponse(404, "Lista de compras não encontrada, excluída ou pertence a outro usuário.", typeof(ErrorResponse))]
    public async Task<IActionResult> GetById(
        [FromRoute, SwaggerParameter("Id único de identificação de uma lista de compras.", Required = true)] int id
    )
    {
        var result = await _shoppingListService.GetByIdAsync(id);

        return Ok(result);
    }

    [HttpGet("paged")]
    [SwaggerOperation(
        Summary = "Lista as listas de compra do usuário com paginação e filtros.",
        Description = "Recupera uma coleção paginada de listas de compras. Permite filtrar, ordenar e controlar o tamanho da página.",
        OperationId = "ShoppingListsPaged",
        Tags = ["Shopping List"]
    )]
    [SwaggerResponse(200, "Página de listas de compras recuperada com sucesso.", typeof(PagedResponse<ProductResponse>))]
    [SwaggerResponse(400, "Parâmetros de paginação ou filtro inválidos.", typeof(ErrorResponse))]
    public async Task<IActionResult> Paged(
        [FromQuery, SwaggerParameter("Filtros para refinar a listagem.", Required = false)] ShoppingListFilterRequest? request,
        [FromQuery, SwaggerParameter("Número da página.", Required = false)] int? pageNumber = 1,
        [FromQuery, SwaggerParameter("Tamanho da página.", Required = false)] int? pageSize = 10
    )
    {
        var filter = _shoppingListService.GetDynamicFilter(request);

        var result = await _shoppingListService.GetPagedAsync(
            filter,
            null,
            pageNumber ?? 1,
            pageSize ?? 10
        );

        return Ok(result);
    }
}
