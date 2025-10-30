namespace FinanceApp.Domain.ValueObjects;
public readonly record struct Id(Guid Value)
{
    public static Id New() => new(Guid.NewGuid());
    public override string ToString() => Value.ToString();
}