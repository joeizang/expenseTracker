using ExpenseTrackerApi.ApiModels;
using ExpenseTrackerApi.Data;
using ExpenseTrackerApi.DomainModels;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTrackerApi.Abstractions;

public class ExpenseTypeRepository : GenericDataRepository<ExpenseType>, IExpenseTypeRepository
{
    private readonly ExpenseTrackerContext _db;
    public ExpenseTypeRepository(ExpenseTrackerContext context) : base(context)
    {
        _db = context;
    }

    public async Task<IEnumerable<ExpenseTypeApiModel>> GetExpenseTypes(CancellationToken cancellationToken = default)
    {
        var queryable = _db.Set<ExpenseType>().AsNoTracking();
        var projected = queryable.ProjectToType<ExpenseTypeApiModel>();
        var result = await projected.ToListAsync(cancellationToken)
            .ConfigureAwait(false);
        return result;
    }

    public async Task<ExpenseTypeApiModel> GetExpenseTypeByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var queryable = _db.Set<ExpenseType>().AsNoTracking().Where(x => x.Id.Equals(id));
        var projected = queryable.ProjectToType<ExpenseTypeApiModel>();
        var result = await projected.SingleOrDefaultAsync(cancellationToken)
            .ConfigureAwait(false) ?? new ExpenseTypeApiModel();
        return result;
    }
}