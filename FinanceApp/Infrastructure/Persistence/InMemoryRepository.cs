using FinanceApp.Application.Abstractions;
using FinanceApp.Domain.Entities;
using FinanceApp.Domain.ValueObjects;

namespace FinanceApp.Infrastructure.Persistence;

public class InMemoryRepository : IRepository
{
    private readonly Dictionary<Id, BankAccount> _acc = new();
    private readonly Dictionary<Id, Category> _cat = new();
    private readonly Dictionary<Id, Operation> _ops = new();

    public IEnumerable<BankAccount> Accounts() => _acc.Values;
    public BankAccount? GetAccount(Id id) => _acc.TryGetValue(id, out var a) ? a : null;
    public void Add(BankAccount acc) => _acc[acc.Id] = acc;
    public void RemoveAccount(Id id) => _acc.Remove(id);

    public IEnumerable<Category> Categories() => _cat.Values;
    public Category? GetCategory(Id id) => _cat.TryGetValue(id, out var c) ? c : null;
    public void Add(Category category) => _cat[category.Id] = category;
    public void RemoveCategory(Id id) => _cat.Remove(id);

    public IEnumerable<Operation> Operations() => _ops.Values;
    public Operation? GetOperation(Id id) => _ops.TryGetValue(id, out var o) ? o : null;
    public void Add(Operation op) => _ops[op.Id] = op;
    public void RemoveOperation(Id id) => _ops.Remove(id);
}