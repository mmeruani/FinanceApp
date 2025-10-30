using FinanceApp.Application.Abstractions;
using FinanceApp.Application.Services.Commands;
using FinanceApp.Application.Services.Decorators;
using FinanceApp.Application.Services.Facades;
using FinanceApp.Domain.Enums;
using FinanceApp.Domain.ValueObjects;
using FinanceApp.Infrastructure.Export;
using FinanceApp.Infrastructure.Import;

namespace FinanceApp.Presentation;

public class ConsoleApp
{
    private readonly AccountsFacade _acc;
    private readonly CategoriesFacade _cat;
    private readonly OperationsFacade _ops;
    private readonly AnalyticsFacade _ana;
    private readonly ExportService _export;
    private readonly CsvImport _import;

    public ConsoleApp(AccountsFacade acc, CategoriesFacade cat, OperationsFacade ops, AnalyticsFacade ana, ExportService export, CsvImport import)
    {
        _acc = acc;
        _cat = cat;
        _ops = ops;
        _ana = ana;
        _export = export;
        _import = import;
    }

    private static T Timed<T>(ICommand cmd, Func<T> runner)
    {
        var sw = System.Diagnostics.Stopwatch.StartNew();
        var res = runner();
        sw.Stop();
        Console.WriteLine($"[{cmd.Name}] {sw.ElapsedMilliseconds} ms");
        return res;
    }

    private static void Timed(ICommand cmd)
    {
        var sw = System.Diagnostics.Stopwatch.StartNew();
        cmd.Execute();
        sw.Stop();
        Console.WriteLine($"[{cmd.Name}] {sw.ElapsedMilliseconds} ms");
    }

    public void Run()
    {
        var createdAccountIds = new List<Id>();
        var createdCategoryIds = new List<Id>();
        var createdOperationIds = new List<Id>();

        while (true)
        {
            Console.WriteLine("\n==== FINANCE APP ====");
            Console.WriteLine("1) Создать счёт");
            Console.WriteLine("2) Переименовать счёт");
            Console.WriteLine("3) Удалить счёт");
            Console.WriteLine("4) Создать категорию");
            Console.WriteLine("5) Переименовать категорию");
            Console.WriteLine("6) Удалить категорию");
            Console.WriteLine("7) Создать операцию");
            Console.WriteLine("8) Обновить операцию");
            Console.WriteLine("9) Удалить операцию");
            Console.WriteLine("A) Аналитика: разница доход/расход");
            Console.WriteLine("B) Аналитика: по категориям");
            Console.WriteLine("E) Экспорт CSV");
            Console.WriteLine("I) Импорт CSV (демо)");
            Console.WriteLine("L) Списки (счета/категории/операции)");
            Console.WriteLine("0) Выход");
            Console.Write("> ");
            var cmd = Console.ReadLine()?.Trim();

            try
            {
                switch (cmd?.ToUpperInvariant())
                {
                    case "1":
                    {
                        Console.Write("Название счёта: ");
                        var name = Console.ReadLine()!;
                        Console.Write("Начальный баланс: ");
                        var bal = decimal.Parse(Console.ReadLine()!);

                        var c = new CreateAccountCommand(_acc, name, bal);
                        Timed(c);
                        var id = _acc.List().Last().Id;
                        createdAccountIds.Add(id);
                        Console.WriteLine($"Создан счёт: {id}");
                        break;
                    }
                    case "2":
                    {
                        var id = PickId("Выберите счёт", _acc.List().Select(a => (a.Id, a.Name)));
                        Console.Write("Новое имя: ");
                        var name = Console.ReadLine()!;
                        Timed(new RenameAccountCommand(_acc, id, name));
                        break;
                    }
                    case "3":
                    {
                        var id = PickId("Выберите счёт", _acc.List().Select(a => (a.Id, a.Name)));
                        Timed(new DeleteAccountCommand(_acc, id));
                        break;
                    }
                    case "4":
                    {
                        Console.Write("Тип (Income/Expense): ");
                        var t = Enum.Parse<CategoryType>(Console.ReadLine()!, true);
                        Console.Write("Название категории: ");
                        var name = Console.ReadLine()!;
                        Timed(new CreateCategoryCommand(_cat, t, name));
                        var id = _cat.List().Last().Id;
                        createdCategoryIds.Add(id);
                        Console.WriteLine($"Создана категория: {id}");
                        break;
                    }
                    case "5":
                    {
                        var id = PickId("Выберите категорию", _cat.List().Select(c => (c.Id, c.Name)));
                        Console.Write("Новое имя: ");
                        var name = Console.ReadLine()!;
                        Timed(new RenameCategoryCommand(_cat, id, name));
                        break;
                    }
                    case "6":
                    {
                        var id = PickId("Выберите категорию",_cat.List().Select(c => (c.Id, c.Name)));
                        Timed(new DeleteCategoryCommand(_cat, id));
                        break;
                    }
                    case "7":
                    {
                        if (!_acc.List().Any() || !_cat.List().Any())
                        {
                            Console.WriteLine("Нужны хотя бы один счёт и одна категория.");
                            break;
                        }

                        Console.Write("Тип операции (Income/Expense): ");
                        var type = Enum.Parse<CategoryType>(Console.ReadLine()!, true);
                        var accId = PickId("Выберите счёт", _acc.List().Select(a => (a.Id, a.Name)));
                        var catId = PickId("Выберите категорию", _cat.List().Select(c => (c.Id, c.Name)));
                        Console.Write("Сумма (>0): ");
                        var amount = decimal.Parse(Console.ReadLine()!);
                        Console.Write("Дата (yyyy-MM-dd): ");
                        var date = DateTime.Parse(Console.ReadLine()!);
                        Console.Write("Описание (необязательно): ");
                        var d = Console.ReadLine();

                        var c = new CreateOperationCommand(_ops, type, accId, catId, amount, date, d);
                        Timed(c);
                        var id = _ops.List().Last().Id;
                        createdOperationIds.Add(id);
                        Console.WriteLine($"Создана операция: {id}");
                        break;
                    }
                    case "8":
                    {
                        if (!_ops.List().Any())
                        {
                            Console.WriteLine("Нет операций.");
                            break;
                        }

                        var id = PickId("Выберите операцию",_ops.List().Select(o => (o.Id, o.Description ?? "Без описания")));
                        Console.Write("Нов. сумма (или пусто): ");
                        var sAmount = Console.ReadLine();
                        decimal? amount = string.IsNullOrWhiteSpace(sAmount) ? null : decimal.Parse(sAmount);
                        Console.Write("Нов. дата yyyy-MM-dd (или пусто): ");
                        var sDate = Console.ReadLine();
                        DateTime? date = string.IsNullOrWhiteSpace(sDate) ? null : DateTime.Parse(sDate);
                        Console.Write("Нов. описание (или пусто): ");
                        var descr = Console.ReadLine();
                        Console.Write("Новая категория Id (или пусто): ");
                        var sCat = Console.ReadLine();
                        Id? catId = string.IsNullOrWhiteSpace(sCat) ? null : new Id(Guid.Parse(sCat!));

                        Timed(new UpdateOperationCommand(_ops, id, amount, date, descr, catId));
                        break;
                    }
                    case "9":
                    {
                        if (!_ops.List().Any())
                        {
                            Console.WriteLine("Нет операций.");
                            break;
                        }

                        var id = PickId("Выберите операцию",_ops.List().Select(o => (o.Id, o.Description ?? "Без описания")));
                        Timed(new DeleteOperationCommand(_ops, id));
                        break;
                    }
                    case "A":
                    {
                        Console.Write("От (yyyy-MM-dd): ");
                        var from = DateTime.Parse(Console.ReadLine()!);
                        Console.Write("До (yyyy-MM-dd): ");
                        var to = DateTime.Parse(Console.ReadLine()!);
                        var c = new CalcDifferenceCommand(_ana, from, to, diff => Console.WriteLine($"Δ = {diff}"));
                        Timed(c);
                        break;
                    }
                    case "B":
                    {
                        Console.Write("От (yyyy-MM-dd): ");
                        var from = DateTime.Parse(Console.ReadLine()!);
                        Console.Write("До (yyyy-MM-dd): ");
                        var to = DateTime.Parse(Console.ReadLine()!);
                        var c = new GroupByCategoryCommand(_ana, from, to, rows =>
                        {
                            foreach (var (cat, sum) in rows)
                            {
                                Console.WriteLine($"{cat}: {sum}");
                            }
                        });
                        Timed(c);
                        break;
                    }
                    case "E":
                    {
                        Console.Write("Введите имя файла для экспорта (например export.csv): ");
                        var file = Console.ReadLine();
                        if (string.IsNullOrWhiteSpace(file))
                        {
                            file = "export.csv";
                        }

                        _export.ExportToFile(file, new CsvExportVisitor());
                        Console.WriteLine($"\n Данные экспортированы в файл: {Path.GetFullPath(file)}");
                        break;
                    }
                    case "I":
                    {
                        Console.WriteLine("\nВведите полный путь к файлу для импорта данных (CSV).");
                        Console.WriteLine("Пример: /Users/maria/Desktop/import.csv  или  C:\\Users\\Maria\\Documents\\import.csv");
                        Console.Write("Путь: ");

                        var path = Console.ReadLine()?.Trim();

                        if (string.IsNullOrWhiteSpace(path))
                        {
                            Console.WriteLine("Путь к файлу не может быть пустым.");
                            break;
                        }

                        if (!File.Exists(path))
                        {
                            Console.WriteLine($"Файл не найден по указанному пути: {path}");
                            break;
                        }

                        try
                        {
                            var repo = GetRepo();
                            var factory = GetFactory();
                            var uow = GetUow();

                            _import.ImportFromFile(path, repo, factory, uow);
                            Console.WriteLine($"\nИмпорт успешно выполнен из файла: {Path.GetFullPath(path)}");
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine($"Ошибка при импорте: {e.Message}");
                        }
                        break;
                    }

                    case "L":
                    {
                        Console.WriteLine("\nСЧЕТА:");
                        foreach (var a in _acc.List())
                        {
                            Console.WriteLine($"{a.Id}  {a.Name}  Баланс={a.Balance}");
                        }

                        Console.WriteLine("КАТЕГОРИИ:");
                        foreach (var c in _cat.List())
                        {
                            Console.WriteLine($"{c.Id}  {c.Type}  {c.Name}");
                        }

                        Console.WriteLine("ОПЕРАЦИИ:");
                        foreach (var o in _ops.List())
                        {
                            Console.WriteLine(
                                $"{o.Id}  {o.Type}  acc={o.BankAccountId} cat={o.CategoryId} amount={o.Amount} date={o.Date:yyyy-MM-dd} '{o.Description}'");
                        }
                        break;
                    }
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Неизвестная команда.");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }

        IRepository GetRepo() => (IRepository)typeof(AccountsFacade)
                                    .GetField("_repo", System.Reflection.BindingFlags.NonPublic|System.Reflection.BindingFlags.Instance)!
                                    .GetValue(_acc);
        Domain.Factory.DomainFactory GetFactory() => (Domain.Factory.DomainFactory)typeof(AccountsFacade)
                                    .GetField("_factory", System.Reflection.BindingFlags.NonPublic|System.Reflection.BindingFlags.Instance)!
                                    .GetValue(_acc);
        Application.Abstractions.IUnitOfWork GetUow() => (Application.Abstractions.IUnitOfWork)typeof(AccountsFacade)
                                    .GetField("_uow", System.Reflection.BindingFlags.NonPublic|System.Reflection.BindingFlags.Instance)!
                                    .GetValue(_acc);
    }

    private static Id PickId(string prompt, IEnumerable<(Id Id, string Name)> items)
    {
        var list = items.ToList();
        if (!list.Any())
        {
            throw new InvalidOperationException("Нет доступных элементов. Сначала создайте хотя бы один.");
        }

        Console.WriteLine($"\n{prompt}:");

        for (int i = 0; i < list.Count; i++)
        {
            Console.WriteLine($"{i + 1}) {list[i].Name}");
        }

        Console.Write("> ");
        if (!int.TryParse(Console.ReadLine(), out var choice) || choice < 1 || choice > list.Count)
        {
            throw new ArgumentException("Некорректный выбор.");
        }

        return list[choice - 1].Id;
    }


}
