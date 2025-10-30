using FinanceApp.Application.Abstractions;

namespace FinanceApp.Infrastructure.Persistence;

public class UnitOfWork : IUnitOfWork
{
    public void Commit() { /* InMemory: no-op. Для БД: SaveChanges() */ }
}