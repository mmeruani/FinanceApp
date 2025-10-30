using FinanceApp.Application.Abstractions;
using FinanceApp.Application.Services.Facades;
using FinanceApp.Domain.ValueObjects;

namespace FinanceApp.Application.Services.Commands;
public sealed class DeleteCategoryCommand : ICommand
{
    private readonly CategoriesFacade _facade;
    private readonly Id _id;
    public string Name => "DeleteCategory";

    public DeleteCategoryCommand(CategoriesFacade f, Id id)
    {
        _facade = f;
        _id = id;
    }
    public void Execute() => _facade.Delete(_id);
}