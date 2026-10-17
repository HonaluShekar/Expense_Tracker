using ExpenseTracker.Models;
using ExpenseTracker.Persistence;
using ExpenseTracker.Services;
using Xunit;

namespace ExpenseTracker.Tests;

public class TransactionServiceTests
{
    [Fact]
    public void AddTransaction_ValidTransaction_AssignsIdAndStoresIt()
    {
        string filePath = Path.Combine(
            Path.GetTempPath(),
            $"expense-test-{Guid.NewGuid()}.json");

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
                Description = "Test salary"
            };

           
            service.AddTransaction(transaction);

          
            Assert.Equal(1, transaction.Id);

            var transactions = service.GetAllTransactions();

            Assert.Single(transactions);
            Assert.Equal("Test salary", transactions[0].Description);
            Assert.Equal(25000, transactions[0].Amount);
        }
        finally
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }
}