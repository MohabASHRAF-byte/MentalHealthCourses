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

        public Task<int> AddInstructor(Instructor instructor);
        public Task DeleteInstructor(int? InstructorId, string Name);
        public Task<(int, IEnumerable<Instructor>)> GetAllInstructors(string? search, int requestPageNumber, int requestPageSize);
        public Task<Instructor> GetInstructorByIdOrName(int? Id, string name);








    }
}
