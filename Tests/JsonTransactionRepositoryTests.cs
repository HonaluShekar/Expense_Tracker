
using ExpenseTracker.Models;
using ExpenseTracker.Persistence;
using Xunit;

namespace ExpenseTracker.Tests;

public class JsonTransactionRepositoryTests
{
    [Fact]
    public void SaveAndLoadTransactions_ShouldPreserveData()
    {
        string path = Path.Combine(Path.GetTempPath(),
            $"{Guid.NewGuid()}.json");

        try
        {
            var repository = new JsonTransactionRepository(path);

            var transactions = new List<Transaction>
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

            repository.SaveTransactions(transactions);
            var loaded = repository.LoadTransactions();

            Assert.Single(loaded);
            Assert.Equal(1, loaded[0].Id);
            Assert.Equal(25000, loaded[0].Amount);
            Assert.Equal("Salary", loaded[0].Description);
        }
        finally
        {
            if (File.Exists(path))
                File.Delete(path);
        }
    }

    [Fact]
    public void LoadTransactions_MissingFile_ShouldReturnEmptyList()
    {
        string path = Path.Combine(Path.GetTempPath(),
            $"{Guid.NewGuid()}.json");

        var repository = new JsonTransactionRepository(path);
        var transactions = repository.LoadTransactions();

        Assert.Empty(transactions);
    }
}
