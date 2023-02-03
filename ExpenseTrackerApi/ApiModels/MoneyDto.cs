namespace ExpenseTrackerApi.ApiModels;

public class MoneyDto
{
    public decimal Amount { get; set; }
    
    public string CurrencyCode { get; set; } = string.Empty;
    
    public string CurrencySymbol { get; set; } = string.Empty;
    
    public string CurrencyName { get; set; } = string.Empty;
    
}