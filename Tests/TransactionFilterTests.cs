
using ExpenseTracker.Models;
using ExpenseTracker.Persistence;
using ExpenseTracker.Services;
using Xunit;

namespace ExpenseTracker.Tests;

public class TransactionReportTests
{
    private TransactionService CreateService()
    {
        string path = Path.GetTempFileName();
        return new TransactionService(new JsonTransactionRepository(path));
    }

    private void AddTestTransactions(TransactionService service)
    {
        service.AddTransaction(new Transaction
        {
            Type = TransactionType.Income,
            Amount = 25000,
            Date = new DateTime(2026, 10, 17),
            Category = TransactionCategory.Salary,
            Description = "Salary"
        });

        service.AddTransaction(new Transaction
        {
            Type = TransactionType.Expense,
            Amount = 500,
            Date = new DateTime(2026, 10, 18),
            Category = TransactionCategory.Food,
            Description = "Lunch"
        });
    }

    [Fact]
    public void FinancialSummary_ShouldCalculateCorrectTotals()
    {
        var service = CreateService();
        AddTestTransactions(service);

        Assert.Equal(25000m, service.GetTotalIncome());
        Assert.Equal(500m, service.GetTotalExpenses());
        Assert.Equal(24500m, service.GetBalance());
        Assert.Equal(500m, service.GetTotalExpensesByCategory(
            TransactionCategory.Food));
    }

    [Fact]
    public void MonthlySummary_ShouldCalculateCorrectTotals()
    {
        var service = CreateService();
        AddTestTransactions(service);

        Assert.Equal(25000m, service.GetMonthlyIncome(2026, 10));
        Assert.Equal(500m, service.GetMonthlyExpenses(2026, 10));
    }
}
