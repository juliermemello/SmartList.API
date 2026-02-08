using SmartList.Application.DTOs.Dashboard;

namespace SmartList.Application.Interfaces;

public interface IDashboardService
{
    Task<DashboardResponse> GetUserDashboardAsync(int days = 30);
}
