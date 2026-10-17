using ExpenseTracker.Models;
using ExpenseTracker.Persistence;
using ExpenseTracker.Services;
using Xunit;

namespace ExpenseTracker.Tests;

public class TransactionUpdateTests
{
    [Fact]
    public void UpdateTransaction_ValidTransaction_ShouldUpdateDetails()
    {
        string filePath = Path.Combine(
            Path.GetTempPath(),
            $"update-test-{Guid.NewGuid()}.json");

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

          
            transaction.Amount = 700;
            transaction.Description = "Dinner";

            bool updated = service.UpdateTransaction(
                transaction.Id,
                transaction);

           
            Assert.True(updated);

            var transactions = service.GetAllTransactions();

            Assert.Single(transactions);
            Assert.Equal(700, transactions[0].Amount);
            Assert.Equal("Dinner", transactions[0].Description);
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