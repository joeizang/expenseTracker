using ExpenseTrackerApi.ApiModels;
using ExpenseTrackerApi.DomainModels;

namespace ExpenseTrackerApi.Abstractions;

public interface IExpenseRepository : IRepository<Expense>
{
    Task<IEnumerable<ExpenseApiModel>> GetManyByDescriptionAsync(string keywords, CancellationToken cancellationToken = default);
    Task<IEnumerable<ExpenseApiModel>> GetManyByDateAsync(DateTime startDate, DateTime endDate, 
        CancellationToken cancellationToken = default);
}