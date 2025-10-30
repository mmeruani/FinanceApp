using FinanceApp.Domain.ValueObjects;

namespace FinanceApp.Domain.Entities;

public class BankAccount
{
    public Id Id { get; }
    public string Name { get; private set; }
    public decimal Balance { get; private set; }

    internal BankAccount(Id id, string name, decimal balance)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Account name required");
        }
        Id = id;
        Name = name;
        Balance = balance;
    }

    public void Rename(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Name cannot be empty");
        }
        Name = name;
    }

    public void Apply(decimal delta) => Balance += delta;
}