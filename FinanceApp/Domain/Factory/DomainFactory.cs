using FinanceApp.Domain.Entities;
using FinanceApp.Domain.Enums;
using FinanceApp.Domain.ValueObjects;

namespace FinanceApp.Domain.Factory;

public class DomainFactory
{
    public BankAccount CreateAccount(string name, decimal initialBalance = 0m) => new(Id.New(), name, initialBalance);

    public Category CreateCategory(CategoryType type, string name) => new(Id.New(), type, name);

    public Operation CreateOperation(CategoryType type, Id accountId, Id categoryId, decimal amount, DateTime date, string? descr = null) => new(Id.New(), type, accountId, categoryId, amount, date, descr);
}