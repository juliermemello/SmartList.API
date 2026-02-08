namespace SmartList.Application.DTOs.Dashboard;

public class DashboardResponse
{
    public decimal TotalSpent { get; set; }
    public int TotalItemsBought { get; set; }
    public List<CategorySpending> SpendingByCategory { get; set; } = new();
    public List<TopItem> MostPurchasedItems { get; set; } = new();
    public List<MonthlyEvolution> MonthlyEvolution { get; init; } = new();
}

public record CategorySpending(string Category, decimal Amount, double Percentage);

public record TopItem(string Product, int TimesPurchased);

public record MonthlyEvolution(string Month, decimal Total);