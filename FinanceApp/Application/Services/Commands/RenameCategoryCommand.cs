using FinanceApp.Application.Abstractions;
using FinanceApp.Application.Services.Facades;
using FinanceApp.Domain.ValueObjects;

namespace FinanceApp.Application.Services.Commands;

public sealed class RenameCategoryCommand : ICommand
{
    private readonly CategoriesFacade _facade;
    private readonly Id _id;
    private readonly string _name;
    public string Name => "RenameCategory";

    public RenameCategoryCommand(CategoriesFacade f, Id id, string name)
    {
        _facade = f;
        _id = id;
        _name = name;
    }
    public void Execute() => _facade.Rename(_id, _name);
}