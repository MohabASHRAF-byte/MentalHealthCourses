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
        public Task<int> CreateAuthorAsync(Author author)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAuthorAsync(int authorId)
        {
            throw new NotImplementedException();
        }

        public Task<Author> GetAuthorByIdAsync(int authorId)
        {
            throw new NotImplementedException();
        }

        public Task<(int TotalCount, IEnumerable<Author>)> GetAuthorsAsync(string? search, int requestPageNumber, int requestPageSize)
        {
            throw new NotImplementedException();
        }

        public Task SaveChangesAsync()
        {
            throw new NotImplementedException();
        }

        public Task UpdateAuthorAsync(Author author)
        {
            throw new NotImplementedException();
        }
    }
}
