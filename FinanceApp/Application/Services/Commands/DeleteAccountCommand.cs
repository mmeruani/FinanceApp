using FinanceApp.Application.Abstractions;
using FinanceApp.Application.Services.Facades;
using FinanceApp.Domain.ValueObjects;

namespace FinanceApp.Application.Services.Commands;
public sealed class DeleteAccountCommand : ICommand
{
    private readonly AccountsFacade _facade;
    private readonly Id _id;
    public string Name => "DeleteAccount";

    public DeleteAccountCommand(AccountsFacade f, Id id)
    {
        _facade = f;
        _id=id;
    }
    public void Execute() => _facade.Delete(_id);
}