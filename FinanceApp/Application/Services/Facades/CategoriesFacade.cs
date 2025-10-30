using FinanceApp.Application.Abstractions;
using FinanceApp.Domain.Entities;
using FinanceApp.Domain.Enums;
using FinanceApp.Domain.Factory;
using FinanceApp.Domain.ValueObjects;

namespace FinanceApp.Application.Services.Facades;

public class CategoriesFacade : IFacade
{
    private readonly IRepository _repo;
    private readonly DomainFactory _factory;
    private readonly IUnitOfWork _uow;

    public CategoriesFacade(IRepository repo, DomainFactory factory, IUnitOfWork uow)
    {
        _repo = repo;
        _factory = factory;
        _uow = uow;
    }

    public Category Create(CategoryType type, string name)
    {
        var category = _factory.CreateCategory(type, name);
        _repo.Add(category);
        _uow.Commit();
        return category;
    }

    public void Rename(Id id, string name)
    {
        var cat = _repo.GetCategory(id) ?? throw new InvalidOperationException("Category not found");
        cat.Rename(name);
        _uow.Commit();
    }

    public void Delete(Id id)
    {
        _repo.RemoveCategory(id);
        _uow.Commit();
    }

    public IEnumerable<Category> List() => _repo.Categories();
}