using FinanceApp.Application.Abstractions;
using FinanceApp.Application.Services.Facades;
using FinanceApp.Domain.Enums;

namespace FinanceApp.Application.Services.Commands;

public sealed class CreateCategoryCommand : ICommand
{
    private readonly CategoriesFacade _facade;
    private readonly CategoryType _type;
    private readonly string _name;

    public string Name => "CreateCategory";

    public CreateCategoryCommand(CategoriesFacade facade, CategoryType type, string name)
    {
        _facade = facade; 
        _type = type; 
        _name = name;
    }

    public void Execute() => _facade.Create(_type, _name);
}