using FinanceApp.Application.Abstractions;
using FinanceApp.Domain.Entities;
using FinanceApp.Domain.ValueObjects;

namespace FinanceApp.Infrastructure.Persistence;

public class CachedRepositoryProxy : IRepository
{
    private readonly IRepository _inner;
    private readonly InMemoryRepository _cache = new();

    public CachedRepositoryProxy(IRepository inner)
    {
        _inner = inner;
        foreach (var a in _inner.Accounts())
        {
            _cache.Add(a);
        }

        foreach (var c in _inner.Categories())
        {
            _cache.Add(c);
        }

        foreach (var o in _inner.Operations())
        {
            _cache.Add(o);
        }
    }

    public IEnumerable<BankAccount> Accounts() => _cache.Accounts();
    public BankAccount? GetAccount(Id id) => _cache.GetAccount(id);

    public void Add(BankAccount acc)
    {
        _inner.Add(acc);
        _cache.Add(acc);
    }

    public void RemoveAccount(Id id)
    {
        _inner.RemoveAccount(id);
        _cache.RemoveAccount(id);
    }

    public IEnumerable<Category> Categories() => _cache.Categories();
    public Category? GetCategory(Id id) => _cache.GetCategory(id);

    public void Add(Category category)
    {
        _inner.Add(category);
        _cache.Add(category);
    }

    public void RemoveCategory(Id id)
    {
        _inner.RemoveCategory(id);
        _cache.RemoveCategory(id);
    }

    public IEnumerable<Operation> Operations() => _cache.Operations();
    public Operation? GetOperation(Id id) => _cache.GetOperation(id);

    public void Add(Operation op)
    {
        _inner.Add(op); 
        _cache.Add(op);
    }

    public void RemoveOperation(Id id)
    {
        _inner.RemoveOperation(id);
        _cache.RemoveOperation(id);
    }
}