using ExpenseTracker.Models;
using ExpenseTracker.Persistence;
using ExpenseTracker.Services;
using Xunit;

namespace ExpenseTracker.Tests;

public class TransactionValidationTests
{
    private TransactionService CreateService()
    {
        string filePath = Path.Combine(
            Path.GetTempPath(),
            $"validation-test-{Guid.NewGuid()}.json");

        var repository = new JsonTransactionRepository(filePath);

        return new TransactionService(repository);
    }

    [Fact]
    public void AddTransaction_ZeroAmount_ShouldThrowException()
    {
        var service = CreateService();

        var transaction = new Transaction
        {
            Type = TransactionType.Expense,
            Amount = 0,
            Date = new DateTime(2026, 10, 17),
            Category = TransactionCategory.Food,
            Description = "Test food"
        };

        var exception = Assert.Throws<ArgumentException>(
            () => service.AddTransaction(transaction));

        Assert.Equal(
            "Amount must be greater than zero.",
            exception.Message);
    }

    [Fact]
    public void AddTransaction_NegativeAmount_ShouldThrowException()
    {
        var service = CreateService();

        var transaction = new Transaction
        {
            Type = TransactionType.Expense,
            Amount = -500,
            Date = new DateTime(2026, 10, 17),
            Category = TransactionCategory.Food,
            Description = "Test food"
        };

        Assert.Throws<ArgumentException>(
            () => service.AddTransaction(transaction));
    }

    [Fact]
    public void AddTransaction_InvalidCategory_ShouldThrowException()
    {
        var service = CreateService();

        var transaction = new Transaction
        {
            Type = TransactionType.Income,
            Amount = 1000,
            Date = new DateTime(2026, 10, 17),
            Category = TransactionCategory.Food,
            Description = "Invalid income"
        };

        Assert.Throws<ArgumentException>(
            () => service.AddTransaction(transaction));
    }

    [Fact]
    public void AddTransaction_InvalidDate_ShouldThrowException()
    {
        var service = CreateService();

        var transaction = new Transaction
        {
            Type = TransactionType.Expense,
            Amount = 500,
            Date = DateTime.MinValue,
            Category = TransactionCategory.Food,
            Description = "Test food"
        };

        Assert.Throws<ArgumentException>(
            () => service.AddTransaction(transaction));
    }

    [Fact]
    public void AddTransaction_EmptyDescription_ShouldThrowException()
    {
        var service = CreateService();

        var transaction = new Transaction
        {
            Type = TransactionType.Expense,
            Amount = 500,
            Date = new DateTime(2026, 10, 17),
            Category = TransactionCategory.Food,
            Description = ""
        };

        Assert.Throws<ArgumentException>(
            () => service.AddTransaction(transaction));
    }

    [Fact]
    public void AddTransaction_WhitespaceDescription_ShouldThrowException()
    {
        var service = CreateService();

        var transaction = new Transaction
        {
            Type = TransactionType.Expense,
            Amount = 500,
            Date = new DateTime(2026, 10, 17),
            Category = TransactionCategory.Food,
            Description = "   "
        };

        Assert.Throws<ArgumentException>(
            () => service.AddTransaction(transaction));
    }
}