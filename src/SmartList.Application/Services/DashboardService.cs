using Ardalis.GuardClauses.Net9;
using Microsoft.EntityFrameworkCore;
using SmartList.Application.DTOs.Dashboard;
using SmartList.Application.Interfaces;
using SmartList.Domain.Entity;
using SmartList.Domain.Interfaces.Repositories;

namespace SmartList.Application.Services;
public class DashboardService : IDashboardService
{
    private readonly IUnitOfWork _uow;

    public DashboardService(IUnitOfWork uow)
    {
        _uow = Guard.Against.Null(uow, nameof(uow));
    }

    public async Task<DashboardResponse> GetUserDashboardAsync(int days = 30)
    {
        var startDate = DateTime.Now.AddDays(-days);

        var boughtItems = (await _uow.Repository<ListItem>().GetAllAsync())
            .AsQueryable()
            .AsNoTracking()
            .Where(x => x.IsChecked && x.UpdatedAt >= startDate && x.Product != null && x.Product.Category != null)
            .Include(i => i.Product!)
            .ThenInclude(i => i.Category!)
            .ToList();

        if (boughtItems == null || !boughtItems.Any())
        {
            return new DashboardResponse
            {
                TotalSpent = 0,
                TotalItemsBought = 0,
                SpendingByCategory = new List<CategorySpending>(),
                MostPurchasedItems = new List<TopItem>(),
                MonthlyEvolution = new List<MonthlyEvolution>()
            };
        }

        var totalSpent = boughtItems.Sum(x => x.TotalPrice);
        var totalItems = boughtItems.Count;

        var spendingByCategory = boughtItems
            .GroupBy(i => i.Product!.Category!.Name!)
            .Select(g => new CategorySpending(
                Category: g.Key,
                Amount: g.Sum(x => x.TotalPrice),
                Percentage: totalSpent > 0 ? (double)(g.Sum(x => x.TotalPrice) / totalSpent) * 100 : 0
            ))
            .OrderByDescending(x => x.Amount)
            .ToList();

        var topItems = boughtItems
            .GroupBy(i => i.Product!.Name)
            .Select(g => new TopItem(
                Product: g.Key,
                TimesPurchased: g.Count()
            ))
            .OrderByDescending(x => x.TimesPurchased)
            .Take(5)
            .ToList();

        var monthlyEvolution = await GetMonthlyEvolutionAsync();

        return new DashboardResponse
        {
            TotalSpent = totalSpent,
            TotalItemsBought = totalItems,
            SpendingByCategory = spendingByCategory,
            MostPurchasedItems = topItems,
            MonthlyEvolution = monthlyEvolution
        };
    }

    private async Task<List<MonthlyEvolution>> GetMonthlyEvolutionAsync()
    {
        var sixMonthsAgo = DateTime.Now.AddMonths(-6);

        var data = (await _uow.Repository<ListItem>().GetAllAsync())
            .AsQueryable()
            .AsNoTracking()
            .Where(x => x.IsChecked && x.UpdatedAt >= sixMonthsAgo && x.Product != null && x.Product.Category != null)
            .Include(i => i.Product!)
            .Select(s => new { s.UpdatedAt, Total = s.TotalPrice })
            .ToList();

        return data
            .GroupBy(g => new { g.UpdatedAt!.Value.Year, g.UpdatedAt.Value.Month })
            .Select(s => new MonthlyEvolution(
                Month: $"{s.Key.Month:D2}/{s.Key.Year}",
                Total: s.Sum(x => x.Total)
            ))
            .OrderBy(x => x.Month)
            .ToList();
    }
}
