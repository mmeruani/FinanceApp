using FinanceApp.Application.Abstractions;
using FinanceApp.Application.Services.Facades;
using FinanceApp.Domain.ValueObjects;

namespace FinanceApp.Application.Services.Commands;
public sealed class UpdateOperationCommand : ICommand
{
    private readonly OperationsFacade _facade;
    private readonly Id _id;
    private readonly decimal? _amount;
    private readonly DateTime? _date;
    private readonly string? _descr;
    private readonly Id? _categoryId;
    public string Name => "UpdateOperation";

    public UpdateOperationCommand(OperationsFacade f, Id id, decimal? amount = null, DateTime? date = null, string? descr = null, Id? categoryId = null)
    {
        _facade = f;
        _id = id; 
        _amount = amount;
        _date = date;
        _descr = descr; 
        _categoryId = categoryId;
    }
    public void Execute() => _facade.Update(_id, _amount, _date, _descr, _categoryId);
}