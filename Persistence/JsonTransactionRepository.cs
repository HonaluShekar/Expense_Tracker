using System.Text.Json;
using ExpenseTracker.Models;
namespace ExpenseTracker.Persistence;
public class JsonTransactionRepository
{
    private readonly string filePath;
    public JsonTransactionRepository(string filePath="transactions.json")
    {
        this.filePath = filePath;
    }
    public void SaveTransactions(List<Transaction> transactions)
    {
        string json = JsonSerializer.Serialize(transactions);
        File.WriteAllText(filePath, json);
        Console.WriteLine("Transactions saved to JSON file.");
    }
    public List<Transaction> LoadTransactions()
    {
        if (!File.Exists(filePath))
        {
            Console.WriteLine("No transactions found. Returning an empty list.");
            return new List<Transaction>();
        }

        string json = File.ReadAllText(filePath);
        List<Transaction>? transactions = JsonSerializer.Deserialize<List<Transaction>>(json);

return transactions ?? new List<Transaction>();
    }
}