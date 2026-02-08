using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using SmartList.Application.DTOs.Dashboard;
using SmartList.Application.Interfaces;
using SmartList.Domain.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace SmartList.API.Controllers;

[ApiVersion("1.0")]
public class DashboardController : BaseController
{
    private readonly IDashboardService _dashboardService;

    public DashboardController(IDashboardService dashboardService)
    {
        _dashboardService = dashboardService;
    }

    [HttpGet]
    [SwaggerOperation(
        Summary = "Obtém métricas e estatísticas consolidadas do usuário.",
        Description = "Calcula o gasto total, evolução mensal e distribuição por categorias baseando-se no histórico de compras concluídas.",
        OperationId = "DashboardGetMetrics",
        Tags = ["Dashboard"]
    )]
    [SwaggerResponse(200, "Dados do dashboard recuperados com sucesso.", typeof(DashboardResponse))]
    [SwaggerResponse(401, "Usuário não autenticado.", typeof(ErrorResponse))]
    public async Task<IActionResult> GetDashboard(
        [FromQuery, SwaggerParameter("Dias de processamento.", Required = false)] int days = 30
    )
    {
        var result = await _dashboardService.GetUserDashboardAsync(days);

        return Ok(result);
    }
}
