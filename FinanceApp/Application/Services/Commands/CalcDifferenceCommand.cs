using FinanceApp.Application.Abstractions;
using FinanceApp.Application.Services.Facades;

namespace FinanceApp.Application.Services.Commands;
public sealed class CalcDifferenceCommand : ICommand
{
    private readonly AnalyticsFacade _facade;
    private readonly DateTime _from;
    private readonly DateTime _to;
    private readonly Action<decimal> _onResult;
    public string Name => "CalcDifference";

    public CalcDifferenceCommand(AnalyticsFacade f, DateTime from, DateTime to, Action<decimal> onResult)
    {
        _facade = f;
        _from = from;
        _to = to; 
        _onResult = onResult;
    }
    public void Execute() => _onResult(_facade.Difference(_from, _to));
}