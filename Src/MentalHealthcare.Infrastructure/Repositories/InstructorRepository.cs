using MentalHealthcare.Domain.Dtos;
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
    public class InstructorRepository(MentalHealthDbContext dbContext) : IInstructorRepository
    {
        public async Task<int> CreateInstructorAsync(Instructor instructor)
        {
            await dbContext.Instructors.AddAsync(instructor);
            await dbContext.SaveChangesAsync();
            return instructor.InstructorId;



        }

        public async Task DeleteInstructorAsync(int instructorId)
        {var instructor = await dbContext.Instructors.FindAsync(instructorId);
            if (instructor == null)
                throw new ResourceNotFound(nameof(instructor), instructorId.ToString());
            dbContext.Instructors.Remove(instructor);
            await dbContext.SaveChangesAsync();}

        public async Task<Instructor> GetInstructorByIdAsync(int instructorId)
        {


            var ins = await dbContext.Instructors
            .Where(a => a.InstructorId == instructorId)
            .Select(a => new Instructor
            {
                InstructorId = a.InstructorId,
                Name = a.Name,
                About = a.About,
                ImageUrl = a.ImageUrl,
                Courses = new List<Domain.Entities.Courses.Course>
                {
                new Domain.Entities.Courses.Course
                {Name = a.Name}


                }


            })
            .FirstOrDefaultAsync();

            if (ins == null)
                throw new ResourceNotFound(nameof(Instructor), instructorId.ToString());

            return ins;








        }

        public async Task<(int TotalCount, IEnumerable<Instructor>)> GetInstructorsAsync(string? search, int requestPageNumber, int requestPageSize)
        {


            // Base query
            var baseQuery = dbContext.Instructors.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                baseQuery = baseQuery.Where(a => a.Name.Contains(search));
            }
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
                    Courses = new List<Domain.Entities.Courses.Course>
                {
                new Domain.Entities.Courses.Course
                {Name = ad.Name}


                }


                })
                .ToListAsync();

            return (totalCount, Instructors);




        }

        public async Task UpdateInstructorAsync(Instructor instructor)
        {
            dbContext.Instructors.Update(instructor);   

            await dbContext.SaveChangesAsync();}



        public async Task SaveChangesAsync()
        {
            await dbContext.SaveChangesAsync();
        }





    }
}