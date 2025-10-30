using Microsoft.Extensions.DependencyInjection;
using FinanceApp.Application.Abstractions;
using FinanceApp.Application.Services.Facades;
using FinanceApp.Domain.Factory;
using FinanceApp.Infrastructure.Persistence;
using FinanceApp.Infrastructure.Export;  
using FinanceApp.Infrastructure.Import;
using FinanceApp.Presentation;

namespace FinanceApp.Composition;
public static class DiConfig
{
    public static IServiceProvider Build()
    {
        var services = new ServiceCollection();
        
        services.AddSingleton<IRepository, InMemoryRepository>();
        services.AddSingleton<IUnitOfWork, UnitOfWork>();
        services.AddSingleton<ExportService>();
        services.AddSingleton<CsvImport>();
        
        services.AddSingleton<DomainFactory>();
        
        services.AddSingleton<AccountsFacade>();
        services.AddSingleton<CategoriesFacade>();
        services.AddSingleton<OperationsFacade>();
        services.AddSingleton<AnalyticsFacade>();

        services.AddSingleton<ConsoleApp>();

        return services.BuildServiceProvider();
    }
}