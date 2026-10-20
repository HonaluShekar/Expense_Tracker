
using ExpenseTracker.Models;
using ExpenseTracker.Persistence;
using ExpenseTracker.Services;
using Xunit;

namespace ExpenseTracker.Tests;

public class TransactionSearchTests
{
    private TransactionService CreateService()
    {
        string filePath = Path.GetTempFileName();
        return new TransactionService(new JsonTransactionRepository(filePath));
    }

    [Fact]
    public void SearchByDescription_ExistingKeyword_ShouldFindTransaction()
    {
        var service = CreateService();

        service.AddTransaction(new Transaction
        {
            Type = TransactionType.Income,
            Amount = 25000,
            Date = new DateTime(2026, 10, 17),
            Category = TransactionCategory.Salary,
            Description = "Monthly Salary"
        });

        var results = service.SearchByDescription("salary");

        Assert.Single(results);
        Assert.Equal("Monthly Salary", results[0].Description);
    }

    [Fact]
    public void SearchByDescription_EmptyKeyword()
    {
        var service = new TransactionService();

        var exception = Assert.Throws<ArgumentException>(
            () => service.SearchByDescription(""));

        Assert.Equal("Search keyword cannot be empty.", exception.Message);
    }
}
