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
    public class MeditationRepository(MentalHealthDbContext dbContext) : IMeditationRepository
    {
        public async Task<int> CreateMeditationAsync(Meditation Meditation)
        {
            await dbContext.Meditations.AddAsync(Meditation);

            await dbContext.SaveChangesAsync();

            return Meditation.MeditationId;
        }

        public async Task DeleteMeditationAsync(int MeditationID)
        {

            var Meditation = await dbContext.Meditations.FindAsync(MeditationID);
            if (Meditation == null)
                throw new ResourceNotFound(
                                   "Article",
                                   "التأمل غير موجود",
                                   MeditationID.ToString()
                               ); dbContext.Meditations.Remove(Meditation);
            await dbContext.SaveChangesAsync();






        }

        public async Task<(int, IEnumerable<Meditation>)> GetAllMeditationsAsync(string? search, int requestPageNumber, int requestPageSize)
        {


            var baseQuery = dbContext.Meditations.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                baseQuery = baseQuery.Where(m => m.Title.Contains(search) || m.Content.Contains(search));
            }

            var totalCount = await baseQuery.CountAsync();

            var meditations = await baseQuery
                .OrderBy(m => m.MeditationId)
                .Skip((requestPageNumber - 1) * requestPageSize)
                .Take(requestPageSize)
                .ToListAsync();

            return (totalCount, meditations);





















        }

        public async Task<Meditation> GetMeditationByIdAsync(int Id)
        {

            var Meditation = await dbContext.Meditations
   .Where(a => a.MeditationId == Id)
   .Select(a => new Meditation
   {
       MeditationId = a.MeditationId,
       Title = a.Title,
       Content = a.Content,
       CreatedDate = a.CreatedDate ,
       Image = a.Image

   })
   .FirstOrDefaultAsync();

            if (Meditation == null)
                throw new ResourceNotFound(
                    "Meditation",
                    "التأمل غير موجود",
                    Id.ToString()
                ); ; return Meditation;







        }

        public async Task<(int TotalCount, IEnumerable<Meditation>)> GetRecentlyAddedMeditationsAsync(int count)
        {

            var totalCount = await dbContext.Meditations.CountAsync();

            var Meditations = await dbContext.Meditations
                .OrderByDescending(a => a.MeditationId)
                .Take(count)
                .Include(a => a.MeditationId) // ✅ Ensure navigation property is loaded
                .ToListAsync();

            return (totalCount, Meditations);
        }

        public async Task SaveChangesAsync()
        {



            await dbContext.SaveChangesAsync();





        }

        public async Task UpdateMeditationAsync(Meditation meditation)
        {
            dbContext.Meditations.Update(meditation);

            await dbContext.SaveChangesAsync();
        }
    }
}
