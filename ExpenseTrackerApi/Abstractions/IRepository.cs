using System.Data;

namespace ExpenseTrackerApi.Abstractions;

public interface IRepository<T> where T : BaseDomainModel
{
    IDbTransaction BeginTransaction();
    Task<TResult> GetAsync<TResult>(Guid id, CancellationToken cancellationToken = default)
        where TResult : class, new();
    Task<IEnumerable<TResult>> GetByDateAsync<TResult>(DateTime date, CancellationToken cancellationToken = default)
        where TResult : class, new();

    Task<IEnumerable<TResult>> GetAllAsync<TResult>(int pageNumber = 1, int pageSize = 10,
        CancellationToken cancellationToken = default)
        where TResult : class, new();
    Task<bool> AddAsync(T entity, CancellationToken cancellationToken = default);
    Task<TResult> UpdateAsync<TResult>(T entity, CancellationToken cancellationToken = default)
        where TResult : class, new();
    Task<bool> DeleteAsync<TResult>(T entity, CancellationToken cancellationToken = default)
        where TResult : class, new();
}