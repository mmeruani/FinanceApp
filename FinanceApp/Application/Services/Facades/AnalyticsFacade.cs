using FinanceApp.Application.Abstractions;
using FinanceApp.Domain.Enums;

namespace FinanceApp.Application.Services.Facades;

public class AnalyticsFacade : IFacade
{
    private readonly IRepository _repo;
    public AnalyticsFacade(IRepository repo) { _repo = repo; }

    public decimal Difference(DateTime from, DateTime to)
    {
        var ops = _repo.Operations().Where(o => o.Date >= from && o.Date <= to);
        var income = ops.Where(o => o.Type == CategoryType.Income).Sum(o => o.Amount);
        var expense = ops.Where(o => o.Type == CategoryType.Expense).Sum(o => o.Amount);
        return income - expense;
    }

    public IEnumerable<(string Category, decimal Sum)> GroupByCategory(DateTime from, DateTime to)
    {
        var cats = _repo.Categories().ToDictionary(c => c.Id, c => c);
        return _repo.Operations()
            .Where(o => o.Date >= from && o.Date <= to)
            .GroupBy(o => o.CategoryId)
            .Select(g => (cats[g.Key].Name, g.Sum(x => x.Amount)));
    }
}