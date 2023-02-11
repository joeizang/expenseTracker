namespace ExpenseTrackerApi.DomainModels.ValueObjects;

public record Money
{
    public Money(decimal amount, Currency currency)
    {
        Amount = amount;
        Currency = currency;
    }

    private Money()
    {
        
    }

    public decimal Amount { get; }

    public Currency Currency { get; }
    
}