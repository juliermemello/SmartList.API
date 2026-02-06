using Ardalis.GuardClauses.Net9;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using SmartList.Application.DTOs.ShoppingListItem;
using SmartList.Application.Interfaces;
using SmartList.Domain.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace SmartList.API.Controllers;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/shopping-list")]
public class ShoppingListItemController : BaseController
{
    private readonly IShoppingListItemService _shoppingListItemService;

    public ShoppingListItemController(IShoppingListService shoppingListService, IShoppingListItemService shoppingListItemService)
    {
        _shoppingListItemService = Guard.Against.Null(shoppingListItemService, nameof(shoppingListItemService));
    }

    [HttpPost("{listId:int}/item")]
    [SwaggerOperation(
        Summary = "Adiciona um novo item à lista de compras.",
        Description = "Insere um produto existente em uma lista específica. O sistema valida se o produto e a lista pertencem ao usuário e incrementa a quantidade caso o item já exista.",
        OperationId = "ShoppingListItemsAdd",
        Tags = ["Shopping List - Itens"]
    )]
    [SwaggerResponse(201, "Item adicionado com sucesso.")]
    [SwaggerResponse(404, "Lista ou Produto não encontrado.", typeof(ErrorResponse))]
    public async Task<IActionResult> Create(
        [FromRoute, SwaggerParameter("Id único de identificação de uma lista de compras.", Required = true)] int listId,
        [FromBody, SwaggerRequestBody("Dados para criação de uma nova lista de compras.", Required = true)] ShoppingListItemCreateRequest request
    )
    {
        var result = await _shoppingListItemService.AddAsync(request);

        return CreatedAtAction(nameof(Create), result);
    }

    [HttpPut("{listId:int}/item/{id:int}")]
    [SwaggerOperation(
        Summary = "Atualiza os detalhes de um item na lista de compras.",
        Description = "Modifica a quantidade ou outras propriedades de um item já existente em uma lista. O sistema valida se o item pertence a uma lista de propriedade do usuário autenticado.",
        OperationId = "ShoppingListItemsUpdate",
        Tags = ["Shopping List - Itens"]
    )]
    [SwaggerResponse(200, "Item da lista atualizado com sucesso.", typeof(ShoppingListItemResponse))]
    [SwaggerResponse(400, "Dados inválidos enviados na requisição.", typeof(ErrorResponse))]
    [SwaggerResponse(404, "Item não encontrado na lista ou usuário sem permissão.", typeof(ErrorResponse))]
    public async Task<IActionResult> Update(
        [FromRoute, SwaggerParameter("Id único de identificação de uma lista de compras.", Required = true)] int listId,
        [FromRoute, SwaggerParameter("Id da lista de compras que será atualizada.", Required = true)] int id,
        [FromBody, SwaggerRequestBody("Dados para atualização de uma lista de compras.", Required = true)] ShoppingListItemUpdateRequest request
    )
    {
        var result = await _shoppingListItemService.UpdateAsync(id, request);

        return Ok(result);
    }

    [HttpDelete("{listId:int}/item/{id:int}")]
    [SwaggerOperation(
        Summary = "Remove um item específico da lista de compras.",
        Description = "Exclui permanentemente um produto de uma lista de compras. Esta operação não remove o produto do catálogo, apenas retira o vínculo dele com a lista informada. Operação restrita ao proprietário da lista.",
        OperationId = "ShoppingListItemsDelete",
        Tags = ["Shopping List - Itens"]
    )]
    [SwaggerResponse(200, "Item removido com sucesso.", typeof(ShoppingListItemResponse))]
    [SwaggerResponse(404, "Item não encontrado na lista ou usuário sem permissão.", typeof(ErrorResponse))]
    [SwaggerResponse(401, "Usuário não autenticado.", typeof(ErrorResponse))]
    public async Task<IActionResult> Remove(
        [FromRoute, SwaggerParameter("Id único de identificação de uma lista de compras.", Required = true)] int listId,
        [FromRoute, SwaggerParameter("Id do item da lista de compras que será excluído.", Required = true)] int id
    )
    {
        var result = await _shoppingListItemService.RemoveAsync(id);

        return Ok(result);
    }

    [HttpGet("{listId}/item")]
    [SwaggerOperation(
        Summary = "Lista todos os itens de uma lista de compras específica.",
        Description = "Recupera a relação de produtos, quantidades e status de compra de uma lista determinada. O sistema valida se a lista pertence ao usuário autenticado antes de retornar os dados.",
        OperationId = "ShoppingListItemsGetAll",
        Tags = ["Shopping List - Itens"]
    )]
    [SwaggerResponse(200, "Itens da lista recuperados com sucesso.", typeof(IEnumerable<ShoppingListItemResponse>))]
    [SwaggerResponse(404, "Lista de compras não encontrada ou acesso negado.", typeof(ErrorResponse))]
    public async Task<IActionResult> List(
        [FromRoute, SwaggerParameter("Id único de identificação de uma lista de compras.", Required = true)] int listId,
        [FromQuery, SwaggerParameter("Filtros para refinar a listagem.", Required = false)] ShoppingListItemFilterRequest? request
    )
    {
        var filter = _shoppingListItemService.GetDynamicFilter(request);

        var result = await _shoppingListItemService.GetAllAsync(filter);

        return Ok(result);
    }

    [HttpGet("{listId}/item/{id:int}")]
    [SwaggerOperation(
        Summary = "Obtém os detalhes de um item específico da lista de compras.",
        Description = "Retorna informações detalhadas de um item (vinculo entre produto e lista), incluindo quantidade e status de compra. O sistema valida se o item pertence a uma lista de propriedade do usuário.",
        OperationId = "ShoppingListItemsGetById",
        Tags = ["Shopping List - Itens"]
    )]
    [SwaggerResponse(200, "Item encontrado e retornado com sucesso.", typeof(ShoppingListItemResponse))]
    [SwaggerResponse(404, "Item não encontrado ou usuário sem permissão de acesso.", typeof(ErrorResponse))]
    public async Task<IActionResult> GetById(
        [FromRoute, SwaggerParameter("Id único de identificação de uma lista de compras.", Required = true)] int listId,
        [FromRoute, SwaggerParameter("Id do item da lista de compras que será resgatado.", Required = true)] int id
    )
    {
        var result = await _shoppingListItemService.GetByIdAsync(id);

        return Ok(result);
    }
}
