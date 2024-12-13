using MentalHealthcare.Domain.Entities;
using MentalHealthcare.Domain.Exceptions;
using MentalHealthcare.Domain.Repositories;
using MentalHealthcare.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthcare.Infrastructure.Repositories
{
    public class AuthorRepository(
        MentalHealthDbContext dbContext,
        ILogger<AuthorRepository> logger

        ) : IAuthorRepository
    {
        public async Task<int> AddAuthor(Author author)
        {await  dbContext.Authors.AddAsync( author );
            await dbContext.SaveChangesAsync(); 
            return author.AuthorId;}

        public async Task DeleteAuthor(int? authorId)
        {
            var RemovedAuthor = await dbContext.Authors.FindAsync(authorId);
            if (RemovedAuthor == null) { throw new ResourceNotFound(nameof(Author), authorId.ToString()); }
            dbContext.Authors.Remove(RemovedAuthor); await dbContext.SaveChangesAsync();

        }

        public async Task<(int, IEnumerable<Author>)> GetAllAuthors(string? search, int requestPageNumber, int requestPageSize)
        { // Base query
            var baseQuery = dbContext.Authors.AsQueryable();

            // No filtering for other cases (get all advertisements)

            // Add Include after filtering
            baseQuery = baseQuery.Include(ad => ad.Name);

            // Total count before pagination
            var totalCount = await baseQuery.CountAsync();

            // Apply ordering and pagination
            var Authors = await baseQuery
                .OrderBy(ad => ad.AuthorId) // Order by ID
                .Skip(requestPageSize * (requestPageSize - 1)) // Pagination: Skip
                .Take(requestPageSize) // Pagination: Take
                .Select(ad => new Author
                {
                    AuthorId = ad.AuthorId,
                    Name = ad.Name,
                    About = ad.About,
                    ImageUrl = ad.ImageUrl,
                    AddedBy = ad.AddedBy,
                    Articles = ad.Articles
                })
                .ToListAsync();

            return (totalCount, Authors);




        }

        public async Task<Author> GetAuthorById(int? Id)
        {
            var Author = await dbContext.Authors.Where(a => a.AuthorId == Id)
            .Select(ad => new Author
             {
                 AuthorId = ad.AuthorId,
                 Name = ad.Name,
                 About = ad.About,
                 ImageUrl = ad.ImageUrl,
                 AddedBy = ad.AddedBy,
                 Articles = ad.Articles
             })
                .FirstOrDefaultAsync();

            if (Author == null)
                throw new ResourceNotFound(nameof(Author), Id.ToString());

            return Author;


        }

        public async Task UpdateAuthorAsync(Author author)
        {
          dbContext.Authors.Update(author);
            await dbContext.SaveChangesAsync();
        }
    }
}
