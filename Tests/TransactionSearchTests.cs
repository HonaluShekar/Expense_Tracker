
using ExpenseTracker.Models;
using ExpenseTracker.Persistence;
using ExpenseTracker.Services;
using Xunit;

namespace ExpenseTracker.Tests;

public class TransactionSearchTests
{
    [Fact]
    public void SearchByDescription_ExistingKeyword_ShouldFindTransaction()
    {
        string filePath = Path.Combine(
            Path.GetTempPath(),
            $"search-test-{Guid.NewGuid()}.json");

        try
        {
            var repository = new JsonTransactionRepository(filePath);
            var service = new TransactionService(repository);

            var transaction = new Transaction
            {
                Type = TransactionType.Income,
                Amount = 25000,
                Date = new DateTime(2026, 10, 17),
                Category = TransactionCategory.Salary,
                Description = "Monthly Salary"
            };

            service.AddTransaction(transaction);

        
            var results = service.SearchByDescription("salary");

           
            Assert.Single(results);
            Assert.Equal("Monthly Salary", results[0].Description);
        }
        finally
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }

    [Fact]
    public void SearchByDescription_EmptyKeyword_ShouldThrowException()
    {
        var service = new TransactionService();

        var exception = Assert.Throws<ArgumentException>(
            () => service.SearchByDescription(""));

        Assert.Equal(
            "Search keyword cannot be empty.",
            exception.Message);
    }
}
