using MentalHealthcare.Domain.Entities;

namespace MentalHealthcare.Domain.Repositories;

public interface IAuthorRepository
{
    
    
    
    public Task<int> CreateAuthorAsync(Author author);
    public Task<Author> GetAuthorByIdAsync(int authorId);
    public Task UpdateAuthorAsync(Author author);
    public Task DeleteAuthorAsync(int authorId);
    public Task<(int TotalCount, IEnumerable<Author>)> GetAuthorsAsync(string? search, int requestPageNumber, int requestPageSize);
    public Task SaveChangesAsync();

    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
}