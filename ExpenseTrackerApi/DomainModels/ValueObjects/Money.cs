namespace ExpenseTrackerApi.DomainModels.ValueObjects;

public record Money
{
    public Money(decimal amount, Currency currency)
    {
        Amount = amount != decimal.MinValue && Amount != decimal.MaxValue ? Amount : 0;
        Currency = currency ?? new Currency("NGN", "Nigerian Naira", "₦");
    }

    public Money()
    {
        Amount = 0;
        Currency = new Currency("NGN", "Nigerian Naira", "₦");
    }

    public decimal Amount { get; }

    public Currency Currency { get; }
    
    public override string ToString()
    {
        return $"{Currency.Symbol}{Amount}";
    }
    
    public ValueTuple<decimal, Currency> ReturnMoney() => (Amount, Currency);
}