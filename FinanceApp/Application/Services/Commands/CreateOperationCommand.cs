using FinanceApp.Application.Abstractions;
using FinanceApp.Application.Services.Facades;
using FinanceApp.Domain.Enums;
using FinanceApp.Domain.ValueObjects;

namespace FinanceApp.Application.Services.Commands;

public sealed class CreateOperationCommand : ICommand
{
    private readonly OperationsFacade _facade;
    private readonly CategoryType _type;
    private readonly Id _accountId;
    private readonly Id _categoryId;
    private readonly decimal _amount;
    private readonly DateTime _date;
    private readonly string? _descr;

    public string Name => "CreateOperation";

    public CreateOperationCommand(OperationsFacade facade, CategoryType type, Id accountId, Id categoryId, decimal amount, DateTime date, string? descr = null)
    {
        _facade = facade; 
        _type = type; 
        _accountId = accountId; 
        _categoryId = categoryId;
        _amount = amount; 
        _date = date; 
        _descr = descr;
    }

    public void Execute() => _facade.Create(_type, _accountId, _categoryId, _amount, _date, _descr);
}