using ExpenseTrackerApi.ApiModels;
using ExpenseTrackerApi.DomainModels;

namespace ExpenseTrackerApi.Abstractions;

public interface IExpenseTypeRepository : IRepository<ExpenseType>
{
    Task<IEnumerable<ExpenseTypeApiModel>> GetExpenseTypes(CancellationToken cancellationToken = default);
    
    Task<ExpenseTypeApiModel> GetExpenseTypeByIdAsync(Guid id, CancellationToken cancellationToken = default);
}