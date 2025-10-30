using FinanceApp.Application.Abstractions;
using FinanceApp.Application.Services.Facades;

namespace FinanceApp.Application.Services.Commands;
public sealed class GroupByCategoryCommand : ICommand
{
    private readonly AnalyticsFacade _facade;
    private readonly DateTime _from;
    private readonly DateTime _to;
    private readonly Action<IEnumerable<(string Category, decimal Sum)>> _onResult;
    public string Name => "GroupByCategory";

    public GroupByCategoryCommand(AnalyticsFacade f, DateTime from, DateTime to, Action<IEnumerable<(string, decimal)>> onResult)
    {
        _facade = f; 
        _from = from;
        _to = to; 
        _onResult = onResult;
    }
    public void Execute() => _onResult(_facade.GroupByCategory(_from, _to));
}