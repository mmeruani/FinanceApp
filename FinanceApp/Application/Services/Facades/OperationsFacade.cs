using FinanceApp.Application.Abstractions;
using FinanceApp.Domain.Entities;
using FinanceApp.Domain.Enums;
using FinanceApp.Domain.Factory;
using FinanceApp.Domain.ValueObjects;

namespace FinanceApp.Application.Services.Facades;

public class OperationsFacade : IFacade
{
    private readonly IRepository _repo;
    private readonly DomainFactory _factory;
    private readonly IUnitOfWork _uow;

    public OperationsFacade(IRepository repo, DomainFactory factory, IUnitOfWork uow)
    {
        _repo = repo;
        _factory = factory;
        _uow = uow;
    }

    public Operation Create(CategoryType type, Id accountId, Id categoryId, decimal amount, DateTime date, string? description = null)
    {
        if (_repo.GetAccount(accountId) is null)
        {
            throw new InvalidOperationException("Account not found");
        }

        if (_repo.GetCategory(categoryId) is null)
        {
            throw new InvalidOperationException("Category not found");
        }

        var op = _factory.CreateOperation(type, accountId, categoryId, amount, date, description);
        _repo.Add(op);
        var acc = _repo.GetAccount(accountId)!;
        acc.Apply(type == CategoryType.Income ? amount : -amount);

        _uow.Commit();
        return op;
    }

    public void Update(Id id, decimal? amount = null, DateTime? date = null, string? description = null, Id? categoryId = null)
    {
        var op = _repo.GetOperation(id) ?? throw new InvalidOperationException("Operation not found");

        if (amount.HasValue)
        {
            op.ChangeAmount(amount.Value);
        }
        if (date.HasValue)
        {
            op.ChangeDate(date.Value);
        }
        if (categoryId.HasValue)
        {
            if (_repo.GetCategory(categoryId.Value) is null)
            {
                throw new InvalidOperationException("Category not found");
            }
            op.ChangeCategory(categoryId.Value);
        }

        if (description != null)
        {
            op.ChangeDescription(description);
        }
        _uow.Commit();
    }

    public void Delete(Id id)
    {
        _repo.RemoveOperation(id);
        _uow.Commit();
    }

    public IEnumerable<Operation> List() => _repo.Operations();
}
