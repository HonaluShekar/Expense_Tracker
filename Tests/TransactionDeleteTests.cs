
using ExpenseTracker.Models;
using ExpenseTracker.Persistence;
using ExpenseTracker.Services;
using Xunit;

namespace ExpenseTracker.Tests;

public class TransactionFilterTests
{
    private TransactionService CreateService()
    {
        string path = Path.GetTempFileName();
        return new TransactionService(new JsonTransactionRepository(path));
    }

    private void AddTransaction(
        TransactionService service,
        TransactionType type,
        decimal amount,
        TransactionCategory category,
        string description,
        int day = 17)
    {
        service.AddTransaction(new Transaction
        {
            Type = type,
            Amount = amount,
            Date = new DateTime(2026, 10, day),
            Category = category,
            Description = description
        });
    }

    [Fact]
    public void GetExpenseTransactions_ShouldReturnOnlyExpenses()
    {
        var service = CreateService();

        AddTransaction(service, TransactionType.Expense, 500,
            TransactionCategory.Food, "Lunch");

        AddTransaction(service, TransactionType.Income, 25000,
            TransactionCategory.Salary, "Salary");

        var results = service.GetExpenseTransactions();

        Assert.Single(results);
        Assert.Equal(TransactionType.Expense, results[0].Type);
    }

    [Fact]
    public void GetTransactionsByCategory_ShouldReturnMatchingCategory()
    {
        var service = CreateService();

        AddTransaction(service, TransactionType.Expense, 500,
            TransactionCategory.Food, "Lunch");

        AddTransaction(service, TransactionType.Expense, 1000,
            TransactionCategory.Transport, "Bus");

        var results = service.GetTransactionsByCategory(
            TransactionCategory.Food);

        Assert.Single(results);
        Assert.Equal(TransactionCategory.Food, results[0].Category);
    }

    [Fact]
    public void GetTransactionsByDateRange_ShouldReturnMatchingDates()
    {
        var service = CreateService();

        AddTransaction(service, TransactionType.Expense, 500,
            TransactionCategory.Food, "Lunch", 17);

        AddTransaction(service, TransactionType.Expense, 700,
            TransactionCategory.Food, "Dinner", 25);

        var results = service.GetTransactionsByDateRange(
            new DateTime(2026, 10, 16),
            new DateTime(2026, 10, 20));

        Assert.Single(results);
        Assert.Equal("Lunch", results[0].Description);
    }
}
