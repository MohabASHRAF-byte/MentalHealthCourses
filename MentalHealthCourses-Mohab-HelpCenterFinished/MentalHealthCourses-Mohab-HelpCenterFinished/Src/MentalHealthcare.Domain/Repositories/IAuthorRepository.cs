using MentalHealthcare.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthcare.Domain.Repositories
{
    public interface IAuthorRepository
    {public Task<int> AddAuthor(Author author);
     public Task DeleteAuthor(int? authorId);
     public Task UpdateAuthorAsync(Author author);
     public Task<(int, IEnumerable<Author>)> GetAllAuthors(string? search, int requestPageNumber, int requestPageSize);
     public Task<Author> GetAuthorById(int? Id);}}
