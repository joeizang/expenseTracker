using ExpenseTrackerApi.Abstractions;

namespace ExpenseTrackerApi.DomainModels;

public class ExpenseType : BaseDomainModel
{
    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;
}