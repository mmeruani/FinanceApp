using FinanceApp.Application.Abstractions;
using FinanceApp.Application.Services.Facades;

namespace FinanceApp.Application.Services.Commands;

public sealed class CreateAccountCommand : ICommand
{
    private readonly AccountsFacade _facade; private readonly string _name; private readonly decimal _initial;
    public string Name => "CreateAccount";

    public CreateAccountCommand(AccountsFacade facade, string name, decimal initial)
    {
        _facade = facade; 
        _name = name; 
        _initial = initial;
    }
    public void Execute() => _facade.Create(_name, _initial);
}