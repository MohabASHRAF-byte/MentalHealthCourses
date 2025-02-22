using MentalHealthcare.Domain.Entities;
using MentalHealthcare.Domain.Exceptions;
using MentalHealthcare.Domain.Repositories;
using MentalHealthcare.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MentalHealthcare.Infrastructure.Repositories;

public class AuthorRepository (MentalHealthDbContext dbContext): IAuthorRepository
{
    public async Task<int> CreateAuthorAsync(Author author)
    {
        await dbContext.Authors.AddAsync(author);
        await dbContext.SaveChangesAsync();
        return author.AuthorId;
    }

    public async Task<Author> GetAuthorByIdAsync(int authorId)
    {
        var author = await dbContext.Authors
            .Where(a => a.AuthorId == authorId)
            .Select(a => new Author
            {
                AuthorId = a.AuthorId,
                Name = a.Name,
                About = a.About,
                ImageUrl = a.ImageUrl,
                Articles = a.Articles.Select(article => new Domain.Entities.Article
                {
                    Title = article.Title,
                }).ToList()
            })
            .FirstOrDefaultAsync();

        if (author == null)
            throw new ResourceNotFound(nameof(Author), authorId.ToString());

        return author;
    }


  public async Task UpdateAuthorAsync(Author author)
    {dbContext.Authors.Update(author);   

        await dbContext.SaveChangesAsync(); }

    public async Task DeleteAuthorAsync(int authorId)
    {
        var author = await dbContext.Authors.FindAsync(authorId);
        if (author == null)
            throw new ResourceNotFound(nameof(author), authorId.ToString());
        dbContext.Authors.Remove(author);
        await dbContext.SaveChangesAsync();
        
        
        
    }

    public async Task<(int TotalCount, IEnumerable<Author>)> GetAuthorsAsync(string? search, int requestPageNumber, int requestPageSize)
    {// Base query
        var baseQuery = dbContext.Authors.AsQueryable();

        // Apply search filter
        if (!string.IsNullOrWhiteSpace(search))
        {
            baseQuery = baseQuery.Where(a => a.Name.Contains(search));
        }

        // Get total count before pagination
        var totalCount = await baseQuery.CountAsync();

        // Apply ordering and pagination
        var authors = await baseQuery
            .OrderBy(a => a.AuthorId) // Order by ID
            .Skip(requestPageSize * (requestPageNumber - 1)) // Pagination: Skip
            .Take(requestPageSize) // Pagination: Take
            .Select(a => new Author
            {
                AuthorId = a.AuthorId,
                Name = a.Name,
                About = a.About,
                ImageUrl = a.ImageUrl,
                Articles = a.Articles.Select(article => new Domain.Entities.Article
                {
                    Title = article.Title,
                }).ToList()
            })
            .ToListAsync();

        return (totalCount, authors);
        
        
        
        
        
        
    }

    public async Task SaveChangesAsync()
    {
        await dbContext.SaveChangesAsync();
    }
}