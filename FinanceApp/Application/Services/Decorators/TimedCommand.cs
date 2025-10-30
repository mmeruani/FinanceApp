using FinanceApp.Application.Abstractions;
using System.Diagnostics;

namespace FinanceApp.Application.Services.Decorators;

public sealed class TimedCommand : ICommand
{
    private readonly ICommand _inner;
    private readonly Action<string, TimeSpan> _onMeasured;
    public string Name => _inner.Name;

    public TimedCommand(ICommand inner, Action<string, TimeSpan> onMeasured)
    {
        _inner = inner;
        _onMeasured = onMeasured;
    }

    public void Execute()
    {
        var sw = Stopwatch.StartNew();
        _inner.Execute();
        sw.Stop();
        _onMeasured(Name, sw.Elapsed);
    }
}