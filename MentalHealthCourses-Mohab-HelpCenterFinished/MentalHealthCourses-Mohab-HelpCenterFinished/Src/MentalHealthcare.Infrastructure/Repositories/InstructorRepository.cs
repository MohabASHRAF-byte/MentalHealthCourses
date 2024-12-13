using MentalHealthcare.Domain.Entities;
using MentalHealthcare.Domain.Exceptions;
using MentalHealthcare.Domain.Repositories;
using MentalHealthcare.Infrastructure.Persistence;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthcare.Infrastructure.Repositories
{public class InstructorRepository(MentalHealthDbContext dbContext ):IInstructorRepository
       {public async Task<int> AddInstructor(Instructor instructor)
        {await dbContext.Instructors.AddAsync(instructor);
         await dbContext.SaveChangesAsync();
         return instructor.InstructorId;}
        public async Task DeleteInstructor(int InstructorId)
        {
            var RemovedInstructor = await dbContext.Instructors.FindAsync(InstructorId);
            if (RemovedInstructor == null) { throw new ResourceNotFound(nameof(Instructor), InstructorId.ToString()); }
            dbContext.Instructors.Remove(RemovedInstructor); 
            await dbContext.SaveChangesAsync();}
        public async Task UpdateInstructorAsync(Instructor instructor) 
        {dbContext.Instructors.Update(instructor);
            await dbContext.SaveChangesAsync();}
        public async Task<Instructor> GetInstructorById(int Id)
        {

            var Instructor = await dbContext.Instructors.Where(a => a.InstructorId == Id)
             .Select(ad => new Instructor
             {
                 InstructorId = ad.InstructorId,
                 Name = ad.Name,
                 About = ad.About,
                 ImageUrl = ad.ImageUrl,
                 AddedBy = ad.AddedBy,
                 Courses = ad.Courses
             })
                 .FirstOrDefaultAsync();

            if (Instructor == null)
                throw new ResourceNotFound(nameof(Instructor), Id.ToString());

            return Instructor;




        }
       public async Task<(int, IEnumerable<Instructor>)> GetAllInstructors(string? search, int requestPageNumber, int requestPageSize)
        {

            // Base query
            var baseQuery = dbContext.Instructors.AsQueryable();

            

            // Add Include after filtering
            baseQuery = baseQuery.Include(ad => ad.Name);

            // Total count before pagination
            var totalCount = await baseQuery.CountAsync();

            // Apply ordering and pagination
            var Instructors = await baseQuery
                .OrderBy(ad => ad.InstructorId) // Order by ID
                .Skip(requestPageSize * (requestPageNumber - 1)) // Pagination: Skip
                .Take(requestPageSize) // Pagination: Take
                .Select(ad => new Instructor
                {

                    InstructorId = ad.InstructorId,
                    Name = ad.Name,
                    About = ad.About,
                    ImageUrl = ad.ImageUrl,
                    AddedBy = ad.AddedBy,
                    Courses = ad.Courses
                })
                .ToListAsync();

            return (totalCount, Instructors);






        }}}
