
using ExpenseTracker.Models;
using ExpenseTracker.Persistence;
using ExpenseTracker.Services;
using Xunit;

namespace ExpenseTracker.Tests;

public class TransactionServiceTests
{
    [Fact]
    public void AddTransactionTest()
    {
        string filePath = Path.GetTempFileName();

        try
        {
            var repository = new JsonTransactionRepository(filePath);
            var service = new TransactionService(repository);

            var transaction = new Transaction
            {
                Type = TransactionType.Income,
                Amount = 25000,
                Date = DateTime.Now,
                Category = TransactionCategory.Salary,
                Description = "Test salary"
            };

            service.AddTransaction(transaction);

            Assert.Equal(1, transaction.Id);
            Assert.Single(service.GetAllTransactions());
        }
        finally
        {
            File.Delete(filePath);
        }
    }
}
