
using ExpenseTracker.Models;
using ExpenseTracker.Persistence;
using ExpenseTracker.Services;
using Xunit;

namespace ExpenseTracker.Tests;

public class TransactionValidationTests
{
    private TransactionService CreateService()
    {
        string filePath = Path.GetTempFileName();
        return new TransactionService(new JsonTransactionRepository(filePath));
    }

    [Fact]
    public void AddTransaction_ZeroAmount()
    {
        var service = CreateService();
        var transaction = CreateTransaction(amount: 0);

        var exception = Assert.Throws<ArgumentException>(
            () => service.AddTransaction(transaction));

        Assert.Equal("Amount must be greater than zero.", exception.Message);
    }

    [Fact]
    public void AddTransaction_NegativeAmount()
    {
        var service = CreateService();
        var transaction = CreateTransaction(amount: -500);

        Assert.Throws<ArgumentException>(
            () => service.AddTransaction(transaction));
    }

    [Fact]
    public void AddTransaction_InvalidCategory()
    {
        var service = CreateService();
        var transaction = CreateTransaction(
            type: TransactionType.Income,
            category: TransactionCategory.Food);

        Assert.Throws<ArgumentException>(
            () => service.AddTransaction(transaction));
    }

    [Fact]
    public void AddTransaction_InvalidDate()
    {
        var service = CreateService();
        var transaction = CreateTransaction(date: DateTime.MinValue);

        Assert.Throws<ArgumentException>(
            () => service.AddTransaction(transaction));
    }

    [Fact]
    public void AddTransaction_EmptyDescription()
    {
        var service = CreateService();
        var transaction = CreateTransaction(description: "");

        Assert.Throws<ArgumentException>(
            () => service.AddTransaction(transaction));
    }

    [Fact]
    public void AddTransaction_WhitespaceDescription()
    {
        var service = CreateService();
        var transaction = CreateTransaction(description: "   ");

        Assert.Throws<ArgumentException>(
            () => service.AddTransaction(transaction));
    }

    private Transaction CreateTransaction(
        decimal amount = 500,
        TransactionType type = TransactionType.Expense,
        TransactionCategory category = TransactionCategory.Food,
        DateTime? date = null,
        string description = "Test food")
    {
        return new Transaction
        {
            Type = type,
            Amount = amount,
            Date = date ?? new DateTime(2026, 10, 17),
            Category = category,
            Description = description
        };
    }
}
