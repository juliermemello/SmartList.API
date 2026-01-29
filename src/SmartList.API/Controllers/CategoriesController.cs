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

    [HttpPost("create")]
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
}
