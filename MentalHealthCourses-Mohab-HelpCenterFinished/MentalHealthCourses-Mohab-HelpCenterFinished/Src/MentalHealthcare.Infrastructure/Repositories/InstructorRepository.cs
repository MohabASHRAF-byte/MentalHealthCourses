using MentalHealthcare.Domain.Entities;
using MentalHealthcare.Domain.Exceptions;
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
{
    public class InstructorRepository
        (MentalHealthDbContext dbContext 
        , ILogger<InstructorRepository> logger)
    {


        public async Task<int> AddInstructor(Instructor instructor)
        {

            try
            {
                await dbContext.Instructors.AddAsync(instructor);
                await dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException is SqlException sqlException)
                {
                    // Check for specific foreign key constraint violation (error code 547)
                    foreach (SqlError error in sqlException.Errors)
                    {
                        if (error.Number == 547)
                        {
                            logger.LogError(ex, "Foreign key violation: {Message}", error.Message);
                            throw new ResourceNotFound(nameof(Instructor), instructor.InstructorId.ToString());
                        }
                    }
                }
                logger.LogError(ex, "An error occurred while saving the course.");
                throw;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while saving the course.");
                throw;
            }

            return instructor.InstructorId;




        }



        public async Task DeleteInstructor(int? InstructorId, string Name)
        {
            var Selected_Instructor = await dbContext.Instructors
               .FirstOrDefaultAsync(c => c.InstructorId == InstructorId);

            if (Selected_Instructor == null)
            {
                throw new ResourceNotFound("instructor", InstructorId.ToString());
            }
            dbContext.Instructors.Remove(Selected_Instructor);
            dbContext.SaveChanges();

            logger.LogInformation($"Instructor with ID {InstructorId} has been Deleted successfully!");
        }

        public async Task<(int, IEnumerable<Instructor>)> GetAllInstructors(string? search, int requestPageNumber, int requestPageSize)
        {

            search ??= string.Empty;
            search = search.ToLower();
            var baseQuery = dbContext.Instructors
                .Where(r => r.Name.ToLower().Contains(search));
            var totalCount = await baseQuery.CountAsync();
            var Ins = await baseQuery
                .Skip(requestPageSize * (requestPageNumber - 1))
                .Take(requestPageSize)
                .ToListAsync();
            return (totalCount, Ins);










        }

        public async Task<Instructor> GetInstructorByIdOrName(int? Id, string NameOfInstructor)
        {

            var Selected_Instructor = dbContext.Instructors
             .Include(I => I.Name.ToLower().Contains(NameOfInstructor))
                .FirstOrDefault(P => P.InstructorId == Id);

            if (Selected_Instructor == null)
            { throw new ResourceNotFound(nameof(Instructor), Id.ToString()); }


            return Selected_Instructor;






        }







    }
}
