namespace ExpenseTrackerApi.DomainModels.ValueObjects;

public record Currency
{
    public Currency(string code, string name, string symbol)
    {
        Code = !string.IsNullOrWhiteSpace(code) ? code : throw new ArgumentNullException(nameof(code));
        Name = !string.IsNullOrWhiteSpace(name) ? name : throw new ArgumentNullException(nameof(name));
        Symbol = !string.IsNullOrWhiteSpace(symbol) ? symbol : throw new ArgumentNullException(nameof(symbol));
    }

    public string Symbol { get; } = string.Empty;

    public string Name { get; } = string.Empty;

    public string Code { get; } = string.Empty;
}