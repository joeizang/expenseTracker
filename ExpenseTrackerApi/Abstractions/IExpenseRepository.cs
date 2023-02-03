using ExpenseTrackerApi.DomainModels;

namespace ExpenseTrackerApi.Abstractions;

public interface IExpenseRepository : IRepository<Expense>
{
    Task<IEnumerable<Expense>> GetManyByDescriptionAsync();

}