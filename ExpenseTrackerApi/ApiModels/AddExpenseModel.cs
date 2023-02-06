namespace ExpenseTrackerApi.ApiModels;

public class AddExpenseModel
{
    public DateTime ExpenseDate { get; set; }
    
    public string Description { get; set; } = string.Empty;
    
    public decimal Amount { get; set; }
    
    public string Currency { get; set; } = string.Empty;
}