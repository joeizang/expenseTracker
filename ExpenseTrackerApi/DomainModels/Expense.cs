using ExpenseTrackerApi.Abstractions;
using ExpenseTrackerApi.ApiModels;
using ExpenseTrackerApi.DomainModels.ValueObjects;
using Mapster;

namespace ExpenseTrackerApi.DomainModels;

public class Expense : BaseDomainModel
{
    private List<ExpenseType> _expenseTypes;
    
    private Expense()
    {
        _expenseTypes = new List<ExpenseType>();
    }
    
    public Expense(string description, Money amount, DateTime expenseDate) : this()
    {
        Description = description;
        Amount = amount;
        ExpenseDate = expenseDate;
    }
    
    public DateTime ExpenseDate { get; set; }
    
    public string Description { get; set; } = string.Empty;

    public Money Amount { get; set; }

    public IEnumerable<ExpenseType> ExpenseTypes => _expenseTypes;
    
    public void AddExpenseType(ExpenseType expenseType)
    {
        _expenseTypes.Add(expenseType);
    }
    
    public void RemoveExpenseType(ExpenseType expenseType)
    {
        _expenseTypes.Remove(expenseType);
    }

    public bool CancelExpense()
    {
        if(Id != Guid.Empty) return false; // Expense has been saved. You can only cancel unsaved expenses.
        _expenseTypes.Clear();
        Description = string.Empty;
        Amount = new Money(0m, new Currency("NGN", "Nigerian Naira", "₦"));
        ExpenseDate = default;
        return true;
    }
}