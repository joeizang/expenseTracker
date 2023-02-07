namespace ExpenseTrackerApi.ApiModels;

public class AddExpenseModel
{
    public AddExpenseModel()
    {
        ExpenseTypes = new List<ExpenseTypeApiModel>();
    }
    public DateTime ExpenseDate { get; set; }
    
    public string Description { get; set; } = string.Empty;
    
    public decimal Amount { get; set; }
    
    public string Currency { get; set; } = string.Empty;
    
    public List<ExpenseTypeApiModel> ExpenseTypes { get; set; }
}