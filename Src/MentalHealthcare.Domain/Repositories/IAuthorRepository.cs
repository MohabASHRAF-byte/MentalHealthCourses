using MentalHealthcare.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthcare.Domain.Repositories
{
    public interface IAuthorRepository
    {public Task<int> AddAuthorAsync(Author author);//Create
        public Task DeleteAuthorAsync(int ID);//Delete
        public Task UpdateAuthorAsync(Author author);//Update
        public Task<(int, IEnumerable<Author>)> GetAllAuthorsAsync(string? search, int requestPageNumber, int requestPageSize);
        public Task<Author> GetAuthorById(int AuthorId);//GetByID
        public Task DeleteAuthorImageAsync(int authorId);

 }



}








