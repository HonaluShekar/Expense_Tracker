
using ExpenseTracker.Models;
using ExpenseTracker.Persistence;
using ExpenseTracker.Services;
using Xunit;

namespace ExpenseTracker.Tests;

public class TransactionFilterTests
{
    [Fact]
    public void GetExpenseTransactions_ShouldReturnOnlyExpenses()
    {
        string path = Path.Combine(Path.GetTempPath(),
            $"filter-{Guid.NewGuid()}.json");

        try
        {
            var service = new TransactionService(
                new JsonTransactionRepository(path));

            service.AddTransaction(new Transaction
            {
                Type = TransactionType.Expense,
                Amount = 500,
                Date = new DateTime(2026, 10, 17),
                Category = TransactionCategory.Food,
                Description = "Lunch"
            });

            service.AddTransaction(new Transaction
            {
                Type = TransactionType.Income,
                Amount = 25000,
                Date = new DateTime(2026, 10, 17),
                Category = TransactionCategory.Salary,
                Description = "Salary"
            });

            var results = service.GetExpenseTransactions();

            Assert.Single(results);
            Assert.Equal(TransactionType.Expense, results[0].Type);
        }
        finally
        {
            if (File.Exists(path))
                File.Delete(path);
        }
    }

    [Fact]
    public void GetTransactionsByCategory_ShouldReturnMatchingCategory()
    {
        string path = Path.Combine(Path.GetTempPath(),
            $"category-{Guid.NewGuid()}.json");

        try
        {
            var service = new TransactionService(
                new JsonTransactionRepository(path));

            service.AddTransaction(new Transaction
            {
                Type = TransactionType.Expense,
                Amount = 500,
                Date = new DateTime(2026, 10, 17),
                Category = TransactionCategory.Food,
                Description = "Lunch"
            });

            service.AddTransaction(new Transaction
            {
                Type = TransactionType.Expense,
                Amount = 1000,
                Date = new DateTime(2026, 10, 17),
                Category = TransactionCategory.Transport,
                Description = "Bus"
            });

            var results = service.GetTransactionsByCategory(
                TransactionCategory.Food);

            Assert.Single(results);
            Assert.Equal(TransactionCategory.Food, results[0].Category);
        }
        finally
        {
            if (File.Exists(path))
                File.Delete(path);
        }
    }

    [Fact]
    public void GetTransactionsByDateRange_ShouldReturnMatchingDates()
    {
        string path = Path.Combine(Path.GetTempPath(),
            $"date-filter-{Guid.NewGuid()}.json");

        try
        {
            var service = new TransactionService(
                new JsonTransactionRepository(path));

            service.AddTransaction(new Transaction
            {
                Type = TransactionType.Expense,
                Amount = 500,
                Date = new DateTime(2026, 10, 17),
                Category = TransactionCategory.Food,
                Description = "Lunch"
            });

            service.AddTransaction(new Transaction
            {
                Type = TransactionType.Expense,
                Amount = 700,
                Date = new DateTime(2026, 10, 25),
                Category = TransactionCategory.Food,
                Description = "Dinner"
            });

            var results = service.GetTransactionsByDateRange(
                new DateTime(2026, 10, 16),
                new DateTime(2026, 10, 20));

            Assert.Single(results);
            Assert.Equal("Lunch", results[0].Description);
        }
        finally
        {
            if (File.Exists(path))
                File.Delete(path);
        }
    }
}
