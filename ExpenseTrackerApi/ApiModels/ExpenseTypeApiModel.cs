namespace ExpenseTrackerApi.ApiModels;

public class ExpenseTypeApiModel
{
    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public Guid ExpenseTypeId { get; set; }
}