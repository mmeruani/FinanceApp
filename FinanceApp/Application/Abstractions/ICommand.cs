namespace FinanceApp.Application.Abstractions;

public interface ICommand
{
    string Name { get; }
    void Execute();
}