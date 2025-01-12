using MentalHealthcare.Domain.Entities;
using MentalHealthcare.Domain.Exceptions;
using MentalHealthcare.Domain.Repositories;
using MentalHealthcare.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthcare.Infrastructure.Repositories
{
    public class AuthorRepository(MentalHealthDbContext dbContext) : IAuthorRepository
    {
        public async Task<int> AddAuthorAsync(Author author)
        {

            await dbContext.Authors.AddAsync(author);
            await dbContext.SaveChangesAsync();
            return author.AuthorId;
        }

        public async Task DeleteAuthorAsync(int ID)
        {

            var AuthorD = await dbContext.Authors.FindAsync(ID);
            if (AuthorD == null)
                throw new ResourceNotFound(nameof(Author), ID.ToString());
            dbContext.Authors.Remove(AuthorD);
            await dbContext.SaveChangesAsync();
         }

        public async Task<(int, IEnumerable<Author>)> GetAllAuthorsAsync(string? search, int requestPageNumber, int requestPageSize)
        {


            // Base query
            var baseQuery = dbContext.Authors.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                baseQuery = baseQuery.Where(a => a.Name.Contains(search));
            }
            // Total count before pagination
            var totalCount = await baseQuery.CountAsync();

          

            // Apply ordering and pagination
            var Authors = await baseQuery
                .OrderBy(ad => ad.AuthorId) // Order by ID
                .Skip(requestPageSize * (requestPageNumber - 1)) // Pagination: Skip
                .Take(requestPageSize) // Pagination: Take
                .Select(ad => new Author
                {
                    AuthorId = ad.AuthorId,
                    Name = ad.Name,
                    About = ad.About,
                    ImageUrl = ad.ImageUrl,
                  
                })
                .ToListAsync();

            return (totalCount, Authors);





        }

        public async Task<Author> GetAuthorById(int AuthorId)
        {

            var Author = await dbContext.Authors
           .Where(a => a.AuthorId == AuthorId)
           .Select(a => new Author
           {
               AuthorId = a.AuthorId,
               Name = a.Name,
               About = a.About,
               ImageUrl = a.ImageUrl,
               Articles = a.Articles

               .Select(Ar => new Article { Title = Ar.Title })
                    .ToList()
           })
           .FirstOrDefaultAsync();

            if (Author == null)
                throw new ResourceNotFound(nameof(Author), AuthorId.ToString());

            return Author;








        }

        public async  Task UpdateAuthorAsync(Author author)
        {
            dbContext.Authors.Update(author);
            await dbContext.SaveChangesAsync();
        }


        public async Task DeleteAuthorImageAsync(int authorId)
        {
            var author = await dbContext.Authors.FindAsync(authorId);

            if (author == null)
                throw new ResourceNotFound(nameof(Author), authorId.ToString());

            if (!string.IsNullOrEmpty(author.ImageUrl))
            {
                // Reset the image URL
                author.ImageUrl = null;

                // Update the author
                dbContext.Authors.Update(author);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
