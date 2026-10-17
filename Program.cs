
using System;
using System.Collections.Generic;
using System.Globalization;
using ExpenseTracker.Models;
using ExpenseTracker.Services;

TransactionService service = new TransactionService();

while (true)
{
    Console.WriteLine("\n EXPENSE TRACKER ");
    Console.WriteLine("1. Add Income");
    Console.WriteLine("2. Add Expense");
    Console.WriteLine("3. View All Transactions");
    Console.WriteLine("4. Search Transaction by ID");
    Console.WriteLine("5. Search by Description");
    Console.WriteLine("6. Update Transaction");
    Console.WriteLine("7. Delete Transaction");
    Console.WriteLine("8. Financial Summary");
    Console.WriteLine("9. Category-wise Expenses");
    Console.WriteLine("10. Monthly Summary");
    Console.WriteLine("11. Filter by Income or Expense");
    Console.WriteLine("12. Filter by Category");
    Console.WriteLine("13. Filter by Date Range");
    Console.WriteLine("14. Save Data");
    Console.WriteLine("0. Exit");

    Console.Write("Enter your choice: ");
    string? choice = Console.ReadLine();

    try
    {
        switch (choice)
        {
            case "1":
                AddTransaction(TransactionType.Income);
                break;

            case "2":
                AddTransaction(TransactionType.Expense);
                break;

            case "3":
                service.DisplayTransactions();
                break;

            case "4":
                SearchById();
                break;

            case "5":
                ShowTransactions(
        service.SearchByDescription(
                        ReadText("Enter description keyword: ")));
                break;

            case "6":
                UpdateTransaction();
                break;

            case "7":
                service.DeleteTransaction(ReadInt("Enter ID to delete: "));
                break;

            case "8":
                service.DisplayFinancialSummary();
                break;

            case "9":
                service.DisplayCategoryWiseExpenses();
                break;

            case "10":
                int year = ReadInt("Enter year: ");
                int month = ReadInt("Enter month (1-12): ");

                if (month >= 1 && month <= 12)
                    service.DisplayMonthlySummary(year, month);
                else
                    Console.WriteLine("Invalid month.");
                break;

            case "11":
                FilterByType();
                break;

            case "12":
                FilterByCategory();
                break;

            case "13":
                FilterByDateRange();
                break;

            case "14":
                service.SaveData();
                break;

            case "0":
                service.SaveData();
                Console.WriteLine("Thank you for using Expense Tracker!");
                return;

            default:
                Console.WriteLine("Invalid choice.");
                break;
        }
    }
    catch (ArgumentException ex)
    {
        Console.WriteLine("Error: " + ex.Message);
    }
}


void AddTransaction(TransactionType type)
{
    Transaction transaction = new Transaction();

    transaction.Type = type;
    transaction.Amount = ReadDecimal("Enter amount: ");
    transaction.Date = ReadDate("Enter date (dd-MM-yyyy): ");

    if (!ReadCategory(out TransactionCategory category, type))
        return;

    transaction.Category = category;
    transaction.Description = ReadText("Enter description: ");

    service.AddTransaction(transaction);
}


void SearchById()
{
    int id = ReadInt("Enter transaction ID: ");

    Transaction? transaction = service.GetTransactionById(id);

    if (transaction == null)
    {
        Console.WriteLine("Transaction not found.");
        return;
    }

    ShowTransactions(new List<Transaction> { transaction });
}


void UpdateTransaction()
{
    int id = ReadInt("Enter ID to update: ");

    if (service.GetTransactionById(id) == null)
    {
        Console.WriteLine("Transaction not found.");
        return;
    }

    string typeInput = ReadText("Enter new type (Income/Expense): ");

    if (!Enum.TryParse(
        typeInput, true, out TransactionType newType)
        || !Enum.IsDefined(typeof(TransactionType), newType))
    {
        Console.WriteLine("Invalid transaction type.");
        return;
    }

    Transaction updated = new Transaction();

    updated.Type = newType;
    updated.Amount = ReadDecimal("Enter new amount: ");
    updated.Date = ReadDate("Enter new date (dd-MM-yyyy): ");

    if (!ReadCategory(out TransactionCategory category, newType))
        return;

    updated.Category = category;
    updated.Description = ReadText("Enter new description: ");

    if (service.UpdateTransaction(id, updated))
        Console.WriteLine("Transaction updated successfully!");
}


void ShowTransactions(List<Transaction> transactions)
{
    if (transactions.Count == 0)
    {
        Console.WriteLine("No transactions found.");
        return;
    }

    foreach (Transaction t in transactions)
    {
        Console.WriteLine(
            $"ID: {t.Id} | {t.Type} | ₹{t.Amount} | " +
            $"{t.Category} | {t.Date:dd-MM-yyyy} | {t.Description}");
    }
}


void FilterByType()
{
    string type = ReadText("Enter type (Income/Expense): ");

    if (type.Equals("Income", StringComparison.OrdinalIgnoreCase))
        ShowTransactions(service.GetIncomeTransactions());
    else if (type.Equals("Expense", StringComparison.OrdinalIgnoreCase))
        ShowTransactions(service.GetExpenseTransactions());
    else
        Console.WriteLine("Invalid type.");
}


void FilterByCategory()
{
    if (ReadCategory(out TransactionCategory category))
    {
        ShowTransactions(service.GetTransactionsByCategory(category));
    }
}


void FilterByDateRange()
{
    DateTime start = ReadDate("Enter start date (dd-MM-yyyy): ");
    DateTime end = ReadDate("Enter end date (dd-MM-yyyy): ");

    ShowTransactions(service.GetTransactionsByDateRange(start, end));
}


string ReadText(string message)
{
    Console.Write(message);
    return Console.ReadLine() ?? "";
}


int ReadInt(string message)
{
    Console.Write(message);

    if (int.TryParse(Console.ReadLine(), out int value))
        return value;

    throw new ArgumentException("Enter a valid whole number.");
}


decimal ReadDecimal(string message)
{
    Console.Write(message);

    if (decimal.TryParse(Console.ReadLine(), out decimal value)
        && value > 0)
    {
        return value;
    }

    throw new ArgumentException("Amount must be greater than zero.");
}


DateTime ReadDate(string message)
{
    Console.Write(message);

    if (DateTime.TryParseExact(
        Console.ReadLine(),
        "dd-MM-yyyy",
        CultureInfo.InvariantCulture,
        DateTimeStyles.None,
        out DateTime date))
    {
        return date;
    }

    throw new ArgumentException("Enter the date in dd-MM-yyyy format.");
}


bool ReadCategory(
    out TransactionCategory category,
    TransactionType? type = null)
{
    if (type == TransactionType.Income)
    {
        Console.WriteLine(
            "Income categories: Salary, Business, Investments, OtherIncome");
    }
    else if (type == TransactionType.Expense)
    {
        Console.WriteLine(
            "Expense categories: Food, Transport, Rent, Utilities, " +
            "Shopping, Healthcare, OtherExpenses");
    }
    else
    {
        Console.WriteLine(
            "Categories: Salary, Business, Investments, OtherIncome, " +
            "Food, Transport, Rent, Utilities, Shopping, Healthcare, OtherExpenses");
    }

    Console.Write("Enter category: ");
    string? input = Console.ReadLine();

    if (Enum.TryParse(
        input, true, out TransactionCategory parsed)
        && Enum.IsDefined(typeof(TransactionCategory), parsed))
    {
        bool isIncomeCategory =
            parsed == TransactionCategory.Salary ||
            parsed == TransactionCategory.Business ||
            parsed == TransactionCategory.Investments ||
            parsed == TransactionCategory.OtherIncome;

        bool isExpenseCategory =
            parsed == TransactionCategory.Food ||
            parsed == TransactionCategory.Transport ||
            parsed == TransactionCategory.Rent ||
            parsed == TransactionCategory.Utilities ||
            parsed == TransactionCategory.Shopping ||
            parsed == TransactionCategory.Healthcare ||
            parsed == TransactionCategory.OtherExpenses;

        bool valid =
            type == null ||
            (type == TransactionType.Income && isIncomeCategory) ||
            (type == TransactionType.Expense && isExpenseCategory);

        if (valid)
        {
            category = parsed;
            return true;
        }
    }

    Console.WriteLine("Invalid category for this selection.");
    category = default;
    return false;
}
