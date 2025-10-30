using FinanceApp.Domain.Enums;
using FinanceApp.Domain.ValueObjects;

namespace FinanceApp.Domain.Entities;

public class Category
{
    public Id Id { get; }
    public CategoryType Type { get; }
    public string Name { get; private set; }

    internal Category(Id id, CategoryType type, string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Category name required");
        }
        Id = id;
        Type = type;
        Name = name;
    }

    public void Rename(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Name cannot be empty");
        }
        Name = name;
    }
}