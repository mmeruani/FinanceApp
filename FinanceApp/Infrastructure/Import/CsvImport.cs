using FinanceApp.Domain.Enums;

namespace FinanceApp.Infrastructure.Import;

public sealed class CsvImport : ImportTemplate
{
    protected override (
        IEnumerable<(string Name, decimal Balance)> accounts,
        IEnumerable<(FinanceApp.Domain.Enums.CategoryType Type, string Name)> categories,
        IEnumerable<(FinanceApp.Domain.Enums.CategoryType Type, string AccountName, string CategoryName, decimal Amount, DateTime Date, string? Desc)> ops
        ) Parse(string text)
    {
        // Формат:
        // [accounts]
        // Name,Balance
        // Основной,1000
        // [categories]
        // Type,Name
        // Income,Зарплата
        // Expense,Кафе
        // [operations]
        // Type,AccountName,CategoryName,Amount,Date,Description
        // Expense,Основной,Кафе,350,2025-10-01,Латте
        // Income,Основной,Зарплата,120000,2025-10-05,Октябрь

        var lines = text.Split('\n')
            .Select(l => l.Trim())
            .Where(l => l.Length > 0 && !l.StartsWith('#'))
            .ToList();

        var acc = new List<(string, decimal)>();
        var cat = new List<(CategoryType, string)>();
        var ops = new List<(CategoryType, string, string, decimal, DateTime, string?)>();

        string mode = "";
        foreach (var l in lines)
        {
            if (l.StartsWith("[") && l.EndsWith("]"))
            {
                mode = l.ToLowerInvariant();
                continue;
            }

            if (!l.Contains(','))
            {
                continue;
            }

            var p = l.Split(',');
            if (mode == "[accounts]")
            {
                if (p[0] == "Name")
                {
                    continue;
                }
                acc.Add((p[0], decimal.Parse(p[1])));
            }
            else if (mode == "[categories]")
            {
                if (p[0] == "Type")
                {
                    continue;
                }
                cat.Add((Enum.Parse<CategoryType>(p[0]), p[1]));
            }
            else if (mode == "[operations]")
            {
                if (p[0] == "Type")
                {
                    continue;
                }
                var type = Enum.Parse<CategoryType>(p[0]);
                ops.Add((type, p[1], p[2], decimal.Parse(p[3]), DateTime.Parse(p[4]), p.Length > 5 ? p[5] : null));
            }
        }

        return (acc, cat, ops);
    }
}
