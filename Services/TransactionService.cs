
using ExpenseTracker.Models;

using ExpenseTracker.Persistence;
using System.Text.Json;

namespace ExpenseTracker.Services;

public class TransactionService
{
    private List<Transaction> transactions = new List<Transaction>();

    private int nextId = 1;
private readonly JsonTransactionRepository repository;   
    private bool canSave = true;

    public void AddTransaction(Transaction transaction)
    {
        if (transaction.Amount <= 0)
        {
            throw new ArgumentException( "Amount must be greater than zero.");
        }

        if (string.IsNullOrWhiteSpace(transaction.Description))
        {
            throw new ArgumentException("Description cannot be empty.");
        }

        if (!Enum.IsDefined(typeof(TransactionType), transaction.Type))
        {
            throw new ArgumentException("Invalid transaction type.");
        }

        if (!Enum.IsDefined(typeof(TransactionCategory), transaction.Category))
        {
            throw new ArgumentException("Invalid transaction category.");
        }

        if (transaction.Date == DateTime.MinValue)
        {
            throw new ArgumentException("Transaction date is required.");
        }

        if (!IsCategoryValid(transaction))
        {
            throw new ArgumentException("Category does not match the transaction type.");
        }

        transaction.Id = nextId;
        nextId++;

        transactions.Add(transaction);

        SaveData();
    }

public bool UpdateTransaction(int id, Transaction updatedTransaction)
{
    Transaction? existingTransaction =
        transactions.FirstOrDefault(t => t.Id == id);

    if (existingTransaction == null)
    {
        Console.WriteLine($"Transaction with ID {id} not found.");
        return false;
    }

    if (updatedTransaction.Amount <= 0)
    {
        throw new ArgumentException("Amount must be greater than zero.");
    }

    if (string.IsNullOrWhiteSpace(updatedTransaction.Description))
    {
        throw new ArgumentException("Description cannot be empty.");
    }

    if (!Enum.IsDefined(typeof(TransactionType), updatedTransaction.Type))
    {
        throw new ArgumentException("Invalid transaction type.");
    }

    if (!Enum.IsDefined(typeof(TransactionCategory), updatedTransaction.Category))
    {
        throw new ArgumentException("Invalid transaction category.");
    }

    if (updatedTransaction.Date == DateTime.MinValue)
    {
        throw new ArgumentException("Transaction date is required.");
    }

    if (!IsCategoryValid(updatedTransaction))
    {
        throw new ArgumentException(
            "Category does not match the transaction type.");
    }

    existingTransaction.Amount = updatedTransaction.Amount;
    existingTransaction.Date = updatedTransaction.Date;
    existingTransaction.Category = updatedTransaction.Category;
    existingTransaction.Description = updatedTransaction.Description;
    existingTransaction.Type = updatedTransaction.Type;

    SaveData();

    return true;
}

public List<Transaction> GetAllTransactions()
    {
        return new List<Transaction>(transactions);
    }

    private bool IsCategoryValid(Transaction transaction)
    {
        if (transaction.Type == TransactionType.Income)
        {
            return transaction.Category == TransactionCategory.Salary
                || transaction.Category == TransactionCategory.Business
                || transaction.Category == TransactionCategory.Investments
                || transaction.Category == TransactionCategory.OtherIncome;
        }

        if (transaction.Type == TransactionType.Expense)
        {
            return transaction.Category == TransactionCategory.Food
                || transaction.Category == TransactionCategory.Transport
                || transaction.Category == TransactionCategory.Rent
                || transaction.Category == TransactionCategory.Utilities
                || transaction.Category == TransactionCategory.Shopping
                || transaction.Category == TransactionCategory.Healthcare
                || transaction.Category == TransactionCategory.OtherExpenses;
        }

        return false;
    }
public void DisplayTransactions()
    {
        if(transactions.Count == 0)
        {
            Console.WriteLine("No transactions available.");
            return;
        }
        var sortedTransactions = transactions.OrderBy(t => t.Date);
        foreach (Transaction transaction in sortedTransactions)
        {
            Console.WriteLine($"ID: {transaction.Id}, Amount: {transaction.Amount}, Date: {transaction.Date:dd-MM-yyy}, Category: {transaction.Category}, Description: {transaction.Description}, Type: {transaction.Type}");
        }
    }
public Transaction? GetTransactionById(int id)
    {
        return transactions.FirstOrDefault(t => t.Id == id);
    }
public List<Transaction> GetIncomeTransactions()
    {
        return transactions.Where(t => t.Type == TransactionType.Income).OrderBy(t => t.Date).ToList();
    }

    public List<Transaction> GetExpenseTransactions()
    {
        return transactions.Where(t => t.Type == TransactionType.Expense).OrderBy(t => t.Date).ToList();
    }
public List<Transaction> GetTransactionsByCategory(TransactionCategory category)
    {
        return transactions.Where(t => t.Category == category).OrderBy(t => t.Date).ToList();
    }
public bool DeleteTransaction(int id)
    {
        Transaction? transaction = GetTransactionById(id);

        if (transaction == null)
        {
            Console.WriteLine($"Transaction with ID {id} not found.");
            return false; 
        }

        transactions.Remove(transaction);
        Console.WriteLine($"Transaction with ID {id} has been deleted.");
    SaveData();
        return true; 
    }
public List<Transaction> GetTransactionsByDateRange(
    DateTime startDate,
    DateTime endDate)
{
    if (startDate > endDate)
    {
        throw new ArgumentException(
            "Start date cannot be after end date.");
    }

    return transactions
        .Where(t => t.Date.Date >= startDate.Date
                 && t.Date.Date <= endDate.Date)
        .OrderBy(t => t.Date)
        .ToList();
}
public List<Transaction> SearchByDescription(string keyword)
{
    if (string.IsNullOrWhiteSpace(keyword))
    {
        throw new ArgumentException(
            "Search keyword cannot be empty.");
    }

    return transactions
        .Where(t => t.Description.Contains(
            keyword,
            StringComparison.OrdinalIgnoreCase))
        .OrderBy(t => t.Date)
        .ToList();
}
public decimal GetTotalIncome()
{
    return transactions
        .Where(t => t.Type == TransactionType.Income)
        .Sum(t => t.Amount);
}
public decimal GetTotalExpenses()
{
    return transactions
        .Where(t => t.Type == TransactionType.Expense)
        .Sum(t => t.Amount);

}
public decimal GetBalance()
{
    return GetTotalIncome() - GetTotalExpenses();


}
public void DisplayFinancialSummary()
{
    decimal totalIncome = GetTotalIncome();
    decimal totalExpenses = GetTotalExpenses();
    decimal balance = GetBalance();

    Console.WriteLine(" FINANCIAL SUMMARY ");
    Console.WriteLine($"Total Income: {totalIncome:C}");
    Console.WriteLine($"Total Expenses: {totalExpenses:C}");
    Console.WriteLine($"Balance: {balance:C}");
}
public decimal GetTotalExpensesByCategory(TransactionCategory category)
{
    return transactions
        .Where(t => t.Type == TransactionType.Expense
                 && t.Category == category)
        .Sum(t => t.Amount);
}
public void DisplayCategoryWiseExpenses()
{
    Console.WriteLine("\n CATEGORY WISE EXPENSES ");

    foreach (TransactionCategory category
             in Enum.GetValues(typeof(TransactionCategory)))
    {
        if (!IsCategoryValid(new Transaction
        {
            Type = TransactionType.Expense,
            Category = category
        }))
        {
            continue;
        }

        decimal total = GetTotalExpensesByCategory(category);

Console.WriteLine(category + " : ₹" + total);        
    }

   
}
public decimal GetMonthlyIncome(int year, int month)
{
    return transactions
        .Where(t => t.Type == TransactionType.Income
                 && t.Date.Year == year
                 && t.Date.Month == month)
        .Sum(t => t.Amount);
}
public decimal GetMonthlyExpenses(int year, int month)
{
    return transactions
        .Where(t => t.Type == TransactionType.Expense
                 && t.Date.Year == year
                 && t.Date.Month == month)
        .Sum(t => t.Amount);
}
public void DisplayMonthlySummary(int year, int month)
{
    decimal income = GetMonthlyIncome(year, month);
    decimal expenses = GetMonthlyExpenses(year, month);

    decimal balance = income - expenses;

    Console.WriteLine("\n MONTHLY SUMMARY ");
    Console.WriteLine("Month: " + month + "/" + year);
    Console.WriteLine("Income: ₹" + income);
    Console.WriteLine("Expenses: ₹" + expenses);
    Console.WriteLine("Balance: ₹" + balance);
}
public TransactionService(
 JsonTransactionRepository? repository = null)
    {
        this.repository = repository ?? new JsonTransactionRepository();
        try
        {
            transactions = this.repository.LoadTransactions();
            if (transactions.Count > 0)
            {
                nextId = transactions.Max(t => t.Id) + 1;
            }
            Console.WriteLine("Transactions loaded successfully.");
        }
         catch (JsonException)
    {
        canSave = false;
        Console.WriteLine(
            "Warning: JSON file is corrupted. Saving is disabled.");
    }
 catch (IOException)
    {
        canSave = false;
        Console.WriteLine(
            "Warning: Could not read the data file. Saving is disabled.");
    }
   catch (UnauthorizedAccessException)
    {
        canSave = false;
        Console.WriteLine("Warning: Cannot access the data file. Saving is disabled.");
    }  
}
public bool SaveData()
{
    if (!canSave)
    {
        Console.WriteLine(
 "Warning: Saving is disabled due to previous errors.");

        return false;
    }

    try
    {
        repository.SaveTransactions(transactions);

        Console.WriteLine("Transactions saved successfully.");

        return true;
    }
    catch (IOException)
    {
        Console.WriteLine("Error: Could not write to the data file. Changes not saved.");

        return false;
    }
    catch (UnauthorizedAccessException)
    {
        Console.WriteLine(
"Error: Cannot access the data file. Changes not saved.");

        return false;
    }
}
}
