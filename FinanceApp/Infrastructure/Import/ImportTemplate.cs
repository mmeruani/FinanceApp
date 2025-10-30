using FinanceApp.Application.Abstractions;
using FinanceApp.Domain.Enums;
using FinanceApp.Domain.Factory;
using FinanceApp.Domain.ValueObjects;

namespace FinanceApp.Infrastructure.Import;

public abstract class ImportTemplate
{
    protected abstract (
        IEnumerable<(string Name, decimal Balance)> accounts,
        IEnumerable<(CategoryType Type, string Name)> categories,
        IEnumerable<(CategoryType Type, string AccountName, string CategoryName, decimal Amount, DateTime Date, string? Desc)> ops
    ) Parse(string text);

    
    public void ImportFromFile(string filePath, IRepository repo, DomainFactory factory, IUnitOfWork uow)
    {
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"Файл не найден: {filePath}");
        }

        var text = File.ReadAllText(filePath);
        Import(text, repo, factory, uow);
    }
    
    public void Import(string text, IRepository repo, DomainFactory factory, IUnitOfWork uow)
    {
        var (accounts, categories, ops) = Parse(text);

        var accMap = new Dictionary<string, Id>(StringComparer.OrdinalIgnoreCase);
        foreach (var a in accounts)
        {
            var acc = factory.CreateAccount(a.Name, a.Balance);
            repo.Add(acc);
            accMap[a.Name] = acc.Id;
        }

        var catMap = new Dictionary<string, (Id id, CategoryType type)>(StringComparer.OrdinalIgnoreCase);
        foreach (var c in categories)
        {
            var cat = factory.CreateCategory(c.Type, c.Name);
            repo.Add(cat);
            catMap[c.Name] = (cat.Id, c.Type);
        }

        foreach (var o in ops)
        {
            if (!accMap.TryGetValue(o.AccountName, out var accId))
            {
                throw new InvalidOperationException($"Unknown account '{o.AccountName}'");
            }
            if (!catMap.TryGetValue(o.CategoryName, out var cat))
            {
                throw new InvalidOperationException($"Unknown category '{o.CategoryName}'");
            }

            var op = factory.CreateOperation(o.Type, accId, cat.id, o.Amount, o.Date, o.Desc);
            repo.Add(op);

            var account = repo.GetAccount(accId)!;
            account.Apply(o.Type == CategoryType.Income ? o.Amount : -o.Amount);
        }

        uow.Commit();
    }
}
