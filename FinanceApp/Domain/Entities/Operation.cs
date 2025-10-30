using FinanceApp.Domain.Enums;
using FinanceApp.Domain.ValueObjects;

namespace FinanceApp.Domain.Entities;

public class Operation
{
    public Id Id { get; }
    public CategoryType Type { get; }
    public Id BankAccountId { get; }
    public Id CategoryId { get; private set; }
    public decimal Amount { get; private set; }
    public DateTime Date { get; private set; }
    public string? Description { get; private set; }

    internal Operation(Id id, CategoryType type, Id accountId, Id categoryId, decimal amount, DateTime date, string? description)
    {
        if (amount <= 0)
        {
            throw new ArgumentException("Amount must be positive");
        }
        Id = id;
        Type = type;
        BankAccountId = accountId;
        CategoryId = categoryId;
        Amount = amount;
        Date = date;
        Description = description;
    }
    
    public void ChangeAmount(decimal amount)
    {
        if (amount <= 0)
        {
            throw new ArgumentException("Amount must be positive");
        }
        Amount = amount;
    }
    public void ChangeDate(DateTime date) => Date = date;
    public void ChangeDescription(string? d) => Description = d;
    public void ChangeCategory(Id categoryId) => CategoryId = categoryId;
}