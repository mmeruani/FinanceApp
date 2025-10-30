using Microsoft.Extensions.DependencyInjection;
using FinanceApp.Composition;
using FinanceApp.Presentation;

var sp = DiConfig.Build();
sp.GetRequiredService<ConsoleApp>().Run();