using Ardalis.GuardClauses.Net9;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using SmartList.Application.DTOs.Category;
using SmartList.Application.Interfaces;
using SmartList.Domain.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace SmartList.API.Controllers;

[ApiVersion("1.0")]
public class CategoriesController : BaseController
{
    private readonly ICategoryService _categoryService;

    public CategoriesController(ICategoryService categoryService)
    {
        _categoryService = Guard.Against.Null(categoryService, nameof(categoryService));
    }

    [HttpPost]
    [SwaggerOperation(
        Summary = "Cria uma nova categoria personalizada para o usuário.",
        Description = "Registra uma nova categoria na conta do usuário autenticado. O sistema valida se já existe uma categoria com o mesmo nome para evitar duplicidade.",
        OperationId = "CategoriesCreate",
        Tags = ["Categories"]
    )]
    [SwaggerResponse(201, "Categoria criada com sucesso.", typeof(CategoryResponse))]
    [SwaggerResponse(400, "Dados inválidos ou categoria já existente.", typeof(ErrorResponse))]
    [SwaggerResponse(401, "Usuário não autenticado.", typeof(ErrorResponse))]
    public async Task<IActionResult> Create(
        [FromBody, SwaggerRequestBody("Dados para criação de uma nova categoria.", Required = true)] CategoryCreateRequest request
    )
    {
        var result = await _categoryService.AddAsync(request);

        return CreatedAtAction(nameof(Create), new { id = result.Id }, result);
    }

    [HttpPut("{id:int}")]
    [SwaggerOperation(
        Summary = "Atualiza uma categoria existente.",
        Description = "Modifica os dados de uma categoria específica do usuário autenticado. O sistema valida se o novo nome fornecido já está em uso por outra categoria do mesmo usuário para evitar duplicidade.",
        OperationId = "CategoriesUpdate",
        Tags = ["Categories"]
    )]
    [SwaggerResponse(204, "Categoria atualizada com sucesso.", typeof(CategoryResponse))]
    [SwaggerResponse(400, "Dados inválidos ou novo nome já em uso.", typeof(ErrorResponse))]
    [SwaggerResponse(404, "Categoria não encontrada ou não pertence ao usuário.", typeof(ErrorResponse))]
    public async Task<IActionResult> Update(
        [FromRoute, SwaggerParameter("Id da categoria que será atualizada.", Required = true)] int id,
        [FromBody, SwaggerRequestBody("Dados para atualização de uma categoria.", Required = true)] CategoryUpdateRequest request
    )
    {
        var result = await _categoryService.UpdateAsync(id, request);

        return CreatedAtAction(nameof(Update), new { id = result.Id }, result);
    }

    [HttpDelete("{id:int}")]
    [SwaggerOperation(
        Summary = "Remove uma categoria do usuário.",
        Description = "Realiza a exclusão lógica (Soft Delete) de uma categoria. O registro deixa de ser exibido nas listagens, mas permanece no histórico para integridade de dados. Esta operação só pode ser realizada pelo dono da categoria.",
        OperationId = "CategoriesDelete",
        Tags = ["Categories"]
    )]
    [SwaggerResponse(204, "Categoria removida com sucesso.", typeof(CategoryResponse))]
    [SwaggerResponse(404, "Categoria não encontrada ou já removida.", typeof(ErrorResponse))]
    [SwaggerResponse(401, "Usuário não autenticado.", typeof(ErrorResponse))]
    public async Task<IActionResult> Remove(
        [FromRoute, SwaggerParameter("Id da categoria que será excluída.", Required = true)] int id
    )
    {
        var result = await _categoryService.RemoveAsync(id);

        return CreatedAtAction(nameof(Remove), new { id = result.Id }, result);
    }

    [HttpGet]
    [SwaggerOperation(
        Summary = "Lista todas as categorias cadastradas.",
        Description = "Lista todos os registros de categorias.",
        OperationId = "CategoriesGetAll",
        Tags = ["Categories"]
    )]
    [SwaggerResponse(200, "Lista de categorias exibida com sucesso.", typeof(IEnumerable<CategoryResponse>))]
    [SwaggerResponse(400, "Não foi possível listar as categorias.", typeof(ErrorResponse))]
    public async Task<IActionResult> List(
        [FromQuery, SwaggerParameter("Filtros para refinar a listagem.", Required = false)] CategoryFilterRequest? request
    )
    {
        var filter = _categoryService.GetDynamicFilter(request);

        var result = await _categoryService.GetAllAsync(filter);

        return CreatedAtAction(nameof(List), result);
    }

    [HttpGet("{id:int}")]
    [SwaggerOperation(
        Summary = "Obtém os detalhes de uma categoria pelo Id.",
        Description = "Retorna as informações detalhadas de uma categoria específica.",
        OperationId = "CategoriesGetById",
        Tags = ["Categories"]
    )]
    [SwaggerResponse(200, "Categoria encontrada e retornada com sucesso.", typeof(CategoryResponse))]
    [SwaggerResponse(404, "Categoria não encontrada ou o Id fornecido é inválido.", typeof(ErrorResponse))]
    public async Task<IActionResult> GetById(
        [FromRoute, SwaggerParameter("Id único de identificação de uma categoria.", Required = true)] int id
    )
    {
        var result = await _categoryService.GetByIdAsync(id);

        return CreatedAtAction(nameof(GetById), result);
    }
}
