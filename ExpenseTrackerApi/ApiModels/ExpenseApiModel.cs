using ExpenseTrackerApi.DomainModels.ValueObjects;

namespace ExpenseTrackerApi.ApiModels;

public class ExpenseApiModel
{
    public DateTime ExpenseDate { get; set; }
    
    public string Description { get; set; } = string.Empty;
    
    public MoneyDto Amount { get; set; } = new();
}