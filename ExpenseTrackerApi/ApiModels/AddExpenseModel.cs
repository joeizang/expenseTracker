using Microsoft.AspNetCore.Mvc;

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

public class GetExpenseByDateModel
{
    public DateTime StartDate { get; set; }
    
    public DateTime EndDate { get; set; }
}

public class UpdateExpenseModel
{
    public UpdateExpenseModel()
    {
        ExpenseTypes = new List<ExpenseTypeApiModel>();
    }
    public Guid Id { get; set; }
    
    public DateTime ExpenseDate { get; set; }
    
    public string Description { get; set; } = string.Empty;
    
    public decimal Amount { get; set; }
    
    public string Currency { get; set; } = string.Empty;
    
    public List<ExpenseTypeApiModel> ExpenseTypes { get; set; }
}