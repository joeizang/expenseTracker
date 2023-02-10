using ExpenseTrackerApi.ApiModels;
using ExpenseTrackerApi.Data;
using ExpenseTrackerApi.DomainModels;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTrackerApi.Abstractions;

public class ExpenseRepository : GenericDataRepository<Expense>, IExpenseRepository
{
    private readonly ExpenseTrackerContext _context;
    public ExpenseRepository(ExpenseTrackerContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ExpenseApiModel>> GetManyByDescriptionAsync(string keywords,
        CancellationToken cancellationToken = default)
    {
        var queryable = _context.Expenses.AsNoTracking();
        if (!string.IsNullOrWhiteSpace(keywords))
        {
            queryable = queryable.Where(x => x.Description.Contains(keywords));
        }
        var projected = queryable.ProjectToType<ExpenseApiModel>();
        var result = await projected.ToListAsync(cancellationToken).ConfigureAwait(false);
        return result;
    }

    public async Task<IEnumerable<ExpenseApiModel>> GetManyByDateAsync(GetExpenseByDateModel model, 
        CancellationToken cancellationToken = default)
    {
        var queryable = _context.Expenses.AsNoTracking().Where(x => x.ExpenseDate >= model.StartDate && 
                                                                    x.ExpenseDate <= model.EndDate);
        var projected = queryable.ProjectToType<ExpenseApiModel>();
        var result = await projected.ToListAsync(cancellationToken).ConfigureAwait(false);
        return result;
    }
}