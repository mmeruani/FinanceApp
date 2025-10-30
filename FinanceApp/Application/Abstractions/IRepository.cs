using FinanceApp.Domain.Entities;
using FinanceApp.Domain.ValueObjects;
namespace FinanceApp.Application.Abstractions;
public interface IRepository
{
    IEnumerable<BankAccount> Accounts();
    BankAccount? GetAccount(Id id);
    void Add(BankAccount acc);
    void RemoveAccount(Id id);

    IEnumerable<Category> Categories();
    Category? GetCategory(Id id);
    void Add(Category category);
    void RemoveCategory(Id id);

    IEnumerable<Operation> Operations();
    Operation? GetOperation(Id id);
    void Add(Operation op);
    void RemoveOperation(Id id);
}