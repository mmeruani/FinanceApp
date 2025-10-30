using FinanceApp.Application.Abstractions;
using FinanceApp.Domain.Entities;
using FinanceApp.Domain.Factory;
using FinanceApp.Domain.ValueObjects;

namespace FinanceApp.Application.Services.Facades;
public class AccountsFacade : IFacade
{
    private readonly IRepository _repo;
    private readonly DomainFactory _factory;
    private readonly IUnitOfWork _uow;

    public AccountsFacade(IRepository repo, DomainFactory factory, IUnitOfWork uow)
    {
        _repo = repo; 
        _factory = factory; 
        _uow = uow;
    }

    public BankAccount Create(string name, decimal initial = 0m)
    {
        var acc = _factory.CreateAccount(name, initial);
        _repo.Add(acc); 
        _uow.Commit();
        return acc;
    }

    public void Rename(Id id, string name)
    {
        var acc = _repo.GetAccount(id) ?? throw new InvalidOperationException("Account not found");
        acc.Rename(name); 
        _uow.Commit();
    }

    public void Delete(Id id)
    {
        _repo.RemoveAccount(id); 
        _uow.Commit();
    }

    public IEnumerable<BankAccount> List() => _repo.Accounts();
}