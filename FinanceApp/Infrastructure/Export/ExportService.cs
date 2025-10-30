using FinanceApp.Application.Abstractions;
using FinanceApp.Domain.Entities;

namespace FinanceApp.Infrastructure.Export;

public class ExportService
{
    private readonly IRepository _repo;
    public ExportService(IRepository repo) { _repo = repo; }
    
    public void ExportToFile(string filePath, IExportVisitor visitor)
    {
        foreach (var a in _repo.Accounts())
        {
            visitor.Visit(a);
        }

        foreach (var c in _repo.Categories())
        {
            visitor.Visit(c);
        }

        foreach (var o in _repo.Operations())
        {
            visitor.Visit(o);
        }

        var text = visitor.GetResult();
        File.WriteAllText(filePath, text);
    }
    
    public string ExportAsText(IExportVisitor visitor)
    {
        foreach (var a in _repo.Accounts())
        {
            visitor.Visit(a);
        }

        foreach (var c in _repo.Categories())
        {
            visitor.Visit(c);
        }

        foreach (var o in _repo.Operations())
        {
            visitor.Visit(o);
        }
        return visitor.GetResult();
    }
}