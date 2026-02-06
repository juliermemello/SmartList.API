using Ardalis.GuardClauses.Net9;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using SmartList.Application.DTOs.Product;
using SmartList.Application.Interfaces;
using SmartList.Domain.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace SmartList.API.Controllers;

[ApiVersion("1.0")]
public class ProductsController : BaseController
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = Guard.Against.Null(productService, nameof(productService));
    }

    [HttpPost]
    [SwaggerOperation(
        Summary = "Cria um novo produto no catálogo do usuário.",
        Description = "Registra um novo produto (ex: Arroz, Feijão, Sabonete) vinculado à conta do usuário autenticado. O sistema valida se a categoria informada existe e se já não há um produto com o mesmo nome para evitar duplicidade na lista.",
        OperationId = "ProductsCreate",
        Tags = ["Products"]
    )]
    [SwaggerResponse(201, "Produto criado com sucesso.", typeof(ProductResponse))]
    [SwaggerResponse(400, "Dados inválidos, como categoria inexistente ou nome de produto duplicado.", typeof(ErrorResponse))]
    [SwaggerResponse(401, "Usuário não autenticado.", typeof(ErrorResponse))]
    public async Task<IActionResult> Create(
        [FromBody, SwaggerRequestBody("Dados para criação de um novo produto.", Required = true)] ProductCreateRequest request
    )
    {
        var result = await _productService.AddAsync(request);

        return CreatedAtAction(nameof(Create), result);
    }

    [HttpPut("{id:int}")]
    [SwaggerOperation(
        Summary = "Atualiza um produto existente.",
        Description = "Modifica os dados de um produto específico (como nome ou categoria) pertencente ao usuário autenticado. O sistema valida se o novo nome já está em uso por outro produto do mesmo usuário e garante que a categoria informada seja válida e pertença ao usuário.",
        OperationId = "ProductsUpdate",
        Tags = ["Products"]
    )]
    [SwaggerResponse(200, "Produto atualizado com sucesso.", typeof(ProductResponse))]
    [SwaggerResponse(400, "Dados inválidos, nome já em uso ou categoria inexistente.", typeof(ErrorResponse))]
    [SwaggerResponse(404, "Produto não encontrado ou não pertence ao usuário.", typeof(ErrorResponse))]
    public async Task<IActionResult> Update(
        [FromRoute, SwaggerParameter("Id da categoria que será atualizada.", Required = true)] int id,
        [FromBody, SwaggerRequestBody("Dados para atualização de um produto.", Required = true)] ProductUpdateRequest request
    )
    {
        var result = await _productService.UpdateAsync(id, request);

        return Ok(result);
    }

    [HttpDelete("{id:int}")]
    [SwaggerOperation(
        Summary = "Remove um produto do catálogo.",
        Description = "Realiza a exclusão lógica (Soft Delete) de um produto específico. O item deixará de aparecer em novas buscas e cadastros, mas será preservado em listas de compras já existentes para manter o histórico de consumo do usuário.",
        OperationId = "ProductsDelete",
        Tags = ["Products"]
    )]
    [SwaggerResponse(200, "Produto removido com sucesso.", typeof(ProductResponse))]
    [SwaggerResponse(404, "Produto não encontrado ou já removido.", typeof(ErrorResponse))]
    [SwaggerResponse(401, "Usuário não autenticado.", typeof(ErrorResponse))]
    public async Task<IActionResult> Remove(
        [FromRoute, SwaggerParameter("Id do produto que será excluído.", Required = true)] int id
    )
    {
        var result = await _productService.RemoveAsync(id);

        return Ok(result);
    }

    [HttpGet]
    [SwaggerOperation(
        Summary = "Lista todos os produtos do usuário.",
        Description = "Recupera a coleção de produtos cadastrados pelo usuário autenticado. A listagem aplica automaticamente filtros de segurança para garantir o isolamento de dados e omite itens que sofreram exclusão lógica (Soft Delete).",
        OperationId = "ProductsGetAll",
        Tags = ["Products"]
    )]
    [SwaggerResponse(200, "Lista de produtos retornada com sucesso.", typeof(IEnumerable<ProductResponse>))]
    [SwaggerResponse(400, "Erro na requisição ao listar os produtos.", typeof(ErrorResponse))]
    [SwaggerResponse(401, "Usuário não autenticado.", typeof(ErrorResponse))]
    public async Task<IActionResult> List(
        [FromQuery, SwaggerParameter("Filtros para refinar a listagem.", Required = false)] ProductFilterRequest? request
    )
    {
        var filter = _productService.GetDynamicFilter(request);

        var result = await _productService.GetAllAsync(filter);

        return Ok(result);
    }

    [HttpGet("{id:int}")]
    [SwaggerOperation(
        Summary = "Busca um produto pelo Id.",
        Description = "Recupera os detalhes de um produto específico pertencente ao catálogo do usuário autenticado. O sistema garante que o acesso seja restrito apenas aos itens criados pelo próprio usuário.",
        OperationId = "ProductsGetById",
        Tags = ["Products"]
    )]
    [SwaggerResponse(200, "Produto encontrado e retornado com sucesso.", typeof(ProductResponse))]
    [SwaggerResponse(404, "Produto não encontrado ou o Id fornecido não pertence ao usuário.", typeof(ErrorResponse))]
    [SwaggerResponse(401, "Usuário não autenticado.", typeof(ErrorResponse))]
    public async Task<IActionResult> GetById(
        [FromRoute, SwaggerParameter("Id único de identificação de um produto.", Required = true)] int id
    )
    {
        var result = await _productService.GetByIdAsync(id);

        return Ok(result);
    }

    [HttpGet("paged")]
    [SwaggerOperation(
        Summary = "Lista os produtos do usuário com paginação e filtros.",
        Description = "Recupera uma coleção paginada de produtos. Permite filtrar, ordenar e controlar o tamanho da página.",
        OperationId = "ProductsPaged",
        Tags = ["Products"]
    )]
    [SwaggerResponse(200, "Página de produtos recuperada com sucesso.", typeof(PagedResponse<ProductResponse>))]
    [SwaggerResponse(400, "Parâmetros de paginação ou filtro inválidos.", typeof(ErrorResponse))]
    public async Task<IActionResult> Paged(
        [FromQuery, SwaggerParameter("Filtros para refinar a listagem.", Required = false)] ProductFilterRequest? request,
        [FromQuery, SwaggerParameter("Número da página.", Required = false)] int? pageNumber = 1,
        [FromQuery, SwaggerParameter("Tamanho da página.", Required = false)] int? pageSize = 10
    )
    {
        var filter = _productService.GetDynamicFilter(request);

        var result = await _productService.GetPagedAsync(
            filter,
            x => x.Name,
            pageNumber ?? 1,
            pageSize ?? 10
        );

        return Ok(result);
    }
}
