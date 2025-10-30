using FinanceApp.Domain.Entities;

namespace FinanceApp.Infrastructure.Export;

public interface IExportVisitor
{
    void Visit(BankAccount acc);
    void Visit(Category cat);
    void Visit(Operation op);
    string GetResult();
}