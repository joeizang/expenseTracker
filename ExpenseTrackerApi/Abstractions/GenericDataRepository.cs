using ExpenseTrackerApi.Data;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTrackerApi.Abstractions;

public class GenericDataRepository<T> : IRepository<T> where T : BaseDomainModel, new()
{
    private readonly DbSet<T> _dbSet;
    
    public GenericDataRepository(ExpenseTrackerContext context)
    {
        _dbSet = context.Set<T>();
    }

    public async Task<T> GetAsync(Guid id) =>
        await _dbSet.AsNoTracking().SingleOrDefaultAsync(x => x.Id.Equals(id))
            .ConfigureAwait(false) ?? new T();


    public async Task<T> GetByDateAsync(DateTime date)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<T>> GetAllAsync(int pageNumber = 1, int pageSize = 10)
    {
        throw new NotImplementedException();
    }

    public async Task<T> AddAsync(T entity)
    {
        throw new NotImplementedException();
    }

    public async Task<T> UpdateAsync(T entity)
    {
        throw new NotImplementedException();
    }

    public async Task<T> DeleteAsync(T entity)
    {
        throw new NotImplementedException();
    }
}