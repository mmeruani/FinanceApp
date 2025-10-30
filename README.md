# FinanceApp — система учёта личных финансов

## Цель работы

Разработать кроссплатформенное приложение, демонстрирующее применение **паттернов проектирования (GoF, GRASP)**, принципов **SOLID**, а также **инъекции зависимостей (DI)** в модульной архитектуре.  
Приложение предназначено для управления личными финансами: ведения счетов, категорий расходов и доходов, создания операций и получения аналитических отчётов.

---

## Стек технологий

- **Язык:** C# 12 / .NET 8  
- **Тип проекта:** Console Application  
- **Основные библиотека:**
  - `Microsoft.Extensions.DependencyInjection` — DI-контейнер
- **Работа с файлами:** CSV (импорт/экспорт)
- **IDE:** Rider / Visual Studio / VS Code

---

## Архитектура решения
Проект построен по принципам многослойной архитектуры: Domain → Application → Infrastructure → Presentation.

```csharp
FinanceApp/
├── Domain/
│   ├── Entities/ (BankAccount, Category, Operation)
│   ├── Enums/ (CategoryType)
│   ├── Factory/ (DomainFactory)
│   └── ValueObjects/ (Id)
│
├── Application/
│   ├── Abstractions/ (IRepository, IUnitOfWork, IFacade, ICommand)
│   ├── Services/
│   │   ├── Facades/ (Accounts, Categories, Operations, Analytics)
│   │   ├── Commands/ (Create, Rename, Delete, Update, CalcDifference, GroupByCategory)
│   │   └── Decorators/ (TimedCommand)
│
├── Infrastructure/
│   ├── Persistence/ (InMemoryRepository, CachedRepositoryProxy, UnitOfWork)
│   ├── Import/ (ImportTemplate, CsvImport)
│   └── Export/ (ExportService, CsvExportVisitor)
│
├── Presentation/
│   └── ConsoleApp.cs
│
└── Composition/
    └── DiConfig.cs
```



## Основные доменные сущности

| Сущность | Назначение |
|-----------|-------------|
| **BankAccount** | Финансовый счёт (имя, баланс). |
| **Category** | Категория (`Income` или `Expense`). |
| **Operation** | Финансовая операция (тип, сумма, дата, категория). |
| **DomainFactory** | Создание валидных экземпляров доменных объектов. |
| **Id** | Обёртка над `Guid` (гарантия уникальности и типобезопасности). |

---

## Использованные паттерны проектирования

| Паттерн | Где реализован | Назначение |
|----------|----------------|------------|
| **Facade** | AccountsFacade, CategoriesFacade, OperationsFacade, AnalyticsFacade | Инкапсулируют сценарии работы с данными. |
| **Command** | Классы `*Command` | Каждая бизнес-операция оформлена как команда (`Execute()`). |
| **Decorator** | TimedCommand | Измеряет время выполнения команды. |
| **Template Method** | ImportTemplate, CsvImport | Шаблон процесса импорта данных. |
| **Visitor** | CsvExportVisitor, ExportService | Экспортирует данные разных типов в единый CSV. |
| **Proxy** | CachedRepositoryProxy | Прокси с кэшированием данных репозитория. |
| **Factory** | DomainFactory | Создаёт сущности с проверкой корректности данных. |
| **GRASP / SOLID** | Вся архитектура | Минимальная связность, высокая когезия, DIP. |

---

## Инъекция зависимостей

Все зависимости регистрируются через `Microsoft.Extensions.DependencyInjection`  
(см. `Composition/DiConfig.cs`):

```csharp
services.AddSingleton<IRepository, InMemoryRepository>();
services.AddSingleton<IUnitOfWork, UnitOfWork>();
services.AddSingleton<DomainFactory>();
services.AddSingleton<AccountsFacade>();
services.AddSingleton<CategoriesFacade>();
services.AddSingleton<OperationsFacade>();
services.AddSingleton<AnalyticsFacade>();
services.AddSingleton<ExportService>();
services.AddSingleton<CsvImport>();
services.AddSingleton<ConsoleApp>();
```

## Импорт данных
Паттерны: Template Method + Facade
Реализовано в CsvImport и ImportTemplate.
Пользователь вводит полный путь к файлу CSV — программа считывает, создаёт счета, категории и операции.
```csharp
I
Введите полный путь к файлу для импорта данных (CSV).
Пример: /Users/maria/Desktop/import.csv
Импорт успешно выполнен из файла: /Users/maria/Desktop/import.csv
```

## Экспорт данных
Паттерны: Visitor + Facade
Реализовано в ExportService и CsvExportVisitor.
CSV-файл содержит три раздела:
- [accounts] — счета и балансы
- [categories] — категории доходов и расходов
- [operations] — все операции пользователя
```csharp
E
Введите путь для сохранения файла (например /Users/maria/Desktop/export.csv)
✅ Данные экспортированы в файл: /Users/maria/Desktop/export.csv
```

## Аналитика
Паттерн: Facade
Реализовано в AnalyticsFacade:
- `Difference(from, to)` — разница доходов и расходов.
- `GroupByCategory(from, to)` — суммы по категориям.

## Консольное меню
```csharp
==== FINANCE APP ====
1) Создать счёт
2) Переименовать счёт
3) Удалить счёт
4) Создать категорию
5) Переименовать категорию
6) Удалить категорию
7) Создать операцию
8) Обновить операцию
9) Удалить операцию
A) Аналитика: разница доход/расход
B) Аналитика: по категориям
E) Экспорт CSV
I) Импорт CSV
L) Списки (счета/категории/операции)
0) Выход
```
## Пример файла import.csv
```csv
[accounts]
Name,Balance
Основной,1000
Копилка,5000
[categories]
Type,Name
Income,Зарплата
Expense,Кафе
[operations]
Type,AccountName,CategoryName,Amount,Date,Description
Income,Основной,Зарплата,120000,2025-10-01,Октябрь
Expense,Основной,Кафе,350,2025-10-02,Латте
Expense,Копилка,Кафе,500,2025-10-03,Ужин
```

## Принцип работы
1. Пользователь выбирает пункт меню.
2. Создаётся и выполняется ICommand.
3. Команда обращается к фасаду.
4. Фасад работает с репозиторием и фабрикой.
5. Данные сохраняются в памяти или в CSV.

## Инструкция по запуску
```bash
git clone https://github.com/<user>/FinanceApp.git
cd FinanceApp
dotnet restore
dotnet build
dotnet run --project FinanceApp/FinanceApp.csproj
```
## Автор
- **ФИО:** Медведская Мария Ильинична
- **Курс:** ФКН ВШЭ, Программная инженерия 2 курс, БПИ-246
- **Дисциплина:** Конструирование программного обеспечения
