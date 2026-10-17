
using ExpenseTracker.Models;
using ExpenseTracker.Persistence;
using ExpenseTracker.Services;
using Xunit;

namespace ExpenseTracker.Tests;

public class TransactionDeleteTests
{
    [Fact]
    public void DeleteTransaction_ExistingId_ShouldRemoveTransaction()
    {
        string filePath = Path.Combine(
            Path.GetTempPath(),
            $"delete-test-{Guid.NewGuid()}.json");

        try
        {
            var repository = new JsonTransactionRepository(filePath);
            var service = new TransactionService(repository);

            var transaction = new Transaction
            {
                Type = TransactionType.Expense,
                Amount = 500,
                Date = new DateTime(2026, 10, 17),
                Category = TransactionCategory.Food,
                Description = "Lunch"
            };

            service.AddTransaction(transaction);

            
            service.DeleteTransaction(transaction.Id);

           
            Assert.Empty(service.GetAllTransactions());
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
