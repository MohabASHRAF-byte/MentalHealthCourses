using MentalHealthcare.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthcare.Domain.Repositories
{
    public interface IInstructorRepository
    {

        public Task<int> CreateInstructorAsync(Instructor instructor);
        public Task<Instructor> GetInstructorByIdAsync(int instructorId);
        public Task UpdateInstructorAsync(Instructor instructor);
        public Task DeleteInstructorAsync(int instructorId);
        public Task<(int TotalCount, IEnumerable<Instructor>)> GetInstructorsAsync(string? search, int requestPageNumber, int requestPageSize);
        public Task SaveChangesAsync();








    }
}
