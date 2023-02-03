namespace ExpenseTrackerApi.Abstractions;

public interface IRepository<T> where T : BaseDomainModel
{
    Task<T> GetAsync(Guid id);
    Task<T> GetByDateAsync(DateTime date);
    Task<IEnumerable<T>> GetAllAsync(int pageNumber = 1, int pageSize = 10);
    Task<T> AddAsync(T entity);
    Task<T> UpdateAsync(T entity);
    Task<T> DeleteAsync(T entity);
}