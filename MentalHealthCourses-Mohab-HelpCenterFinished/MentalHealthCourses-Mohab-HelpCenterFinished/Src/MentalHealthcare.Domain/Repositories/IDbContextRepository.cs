namespace MentalHealthcare.Domain.Repositories;

public interface IDbContextRepository<T> where T : class
{
    Task<T> GetByIdAsync(object id);
    Task<IEnumerable<T>> GetAllAsyncGetAllAsync(string? searchName, int requestPageNumber,
        int requestPageSize);
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(object id);
}