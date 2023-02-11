using ExpenseTrackerApi.Data;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTrackerApi.Abstractions;

public class GenericDataRepository<T> : IRepository<T> where T : BaseDomainModel
{
    private readonly DbSet<T> _dbSet;
    private protected readonly ExpenseTrackerContext _context;
    
    public GenericDataRepository(ExpenseTrackerContext context)
    {
        _context = context;
        _dbSet = _context.Set<T>();
    }

    public async Task<TResult> GetAsync<TResult>(Guid id, CancellationToken cancellationToken = default)
        where TResult : class, new()
    {
        var queryable = _dbSet.AsNoTracking().Where(x => x.Id.Equals(id));
        var projected = queryable.ProjectToType<TResult>(new TypeAdapterConfig()
        {
            
        });
        var result = await projected.SingleOrDefaultAsync(cancellationToken)
            .ConfigureAwait(false) ?? new TResult();
        return result;  
    }

    public async Task<IEnumerable<TResult>> GetByDateAsync<TResult>(DateTime date,
        CancellationToken cancellationToken = default)
        where TResult : class, new()
    {
        var queryable = await _dbSet.AsNoTracking().Where(x => 
                x.CreatedAt.Equals(date))
            .ProjectToType<TResult>().ToListAsync(cancellationToken).ConfigureAwait(false);
        return queryable;
    }

    public async Task<IEnumerable<TResult>> GetAllAsync<TResult>(int pageNumber = 1, int pageSize = 10,
        CancellationToken cancellationToken = default) where TResult : class, new()
    {
        var queryable = _dbSet.AsNoTracking()
            .OrderBy(x => x.CreatedAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize);
        var projected = queryable.ProjectToType<TResult>();
        var result = await projected.ToListAsync(cancellationToken)
            .ConfigureAwait(false);
        return result;
    }

    public async Task<bool> AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        _dbSet.Add(entity);
        return await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false) > 0;
    }

    public async Task<TResult> UpdateAsync<TResult>(T entity, CancellationToken cancellationToken = default)
        where TResult : class, new()
    {
        try
        {
            _dbSet.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            //handle concurrency exception
            foreach(var entry in ex.Entries)
            {
                if (entry.Entity is not T)
                {
                    throw new NotSupportedException("Cannot handle concurrency exception at the moment!");
                }
                //databaseEntry is the current value in the database
                var databaseEntry = await entry.GetDatabaseValuesAsync(cancellationToken);
                var freshUpdates = entry.CurrentValues;
                entry.OriginalValues.SetValues(freshUpdates);
                await entry.ReloadAsync(cancellationToken).ConfigureAwait(false);
            }
        }
        return entity.Adapt<TResult>();
    }

    public async Task<bool> DeleteAsync<TResult>(T entity, CancellationToken cancellationToken = default)
        where TResult : class, new()
    {
        entity.IsDeleted = true;
        var result = await UpdateAsync<TResult>(entity, cancellationToken).ConfigureAwait(false);
        return result != null;
    }
}