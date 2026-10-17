
using ExpenseTracker.Models;
using ExpenseTracker.Persistence;
using Xunit;

namespace ExpenseTracker.Tests;

public class JsonTransactionRepositoryTests
{
    [Fact]
    public void SaveAndLoadTransactions_ShouldPreserveData()
    {
        string filePath = Path.Combine(
            Path.GetTempPath(),
            $"json-test-{Guid.NewGuid()}.json");

        try
        {
            var repository = new JsonTransactionRepository(filePath);

            var originalTransactions = new List<Transaction>
            {
                new Transaction
                {
                    Id = 1,
                    Type = TransactionType.Income,
                    Amount = 25000,
                    Date = new DateTime(2026, 10, 17),
                    Category = TransactionCategory.Salary,
                    Description = "Salary"
                }
            };

          
            repository.SaveTransactions(originalTransactions);
            var loadedTransactions = repository.LoadTransactions();

           
            Assert.Single(loadedTransactions);
            Assert.Equal(1, loadedTransactions[0].Id);
            Assert.Equal(25000, loadedTransactions[0].Amount);
            Assert.Equal("Salary", loadedTransactions[0].Description);
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
    public void LoadTransactions_MissingFile_ShouldReturnEmptyList()
    {
        string filePath = Path.Combine(
            Path.GetTempPath(),
            $"missing-test-{Guid.NewGuid()}.json");

        var repository = new JsonTransactionRepository(filePath);

        var transactions = repository.LoadTransactions();

        Assert.Empty(transactions);
    }
}
