using System.Text;
using FinanceApp.Domain.Entities;

namespace FinanceApp.Infrastructure.Export;

public sealed class CsvExportVisitor : IExportVisitor
{
    private readonly StringBuilder _acc = new("Name,Balance\n");
    private readonly StringBuilder _cat = new("Type,Name\n");
    private readonly StringBuilder _ops = new("Type,AccountId,CategoryId,Amount,Date,Description\n");

    public void Visit(BankAccount acc) => _acc.AppendLine($"{acc.Name},{acc.Balance}");
    public void Visit(Category cat) => _cat.AppendLine($"{cat.Type},{cat.Name}");
    public void Visit(Operation op) => _ops.AppendLine($"{op.Type},{op.BankAccountId},{op.CategoryId},{op.Amount},{op.Date:yyyy-MM-dd},{op.Description}");

    public string GetResult()
        => $"[accounts]\n{_acc}\n[categories]\n{_cat}\n[operations]\n{_ops}\n";
}