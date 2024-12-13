using MentalHealthcare.Domain.Entities;
using MentalHealthcare.Domain.Exceptions;
using MentalHealthcare.Domain.Repositories;
using MentalHealthcare.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthcare.Infrastructure.Repositories
{
    public class PodCasterRepository
  (MentalHealthDbContext dbContext) : IPodCasterRepository

    {
        public async Task<int> AddPodCaster(PodCaster podCaster)
        {

   await dbContext.PodCasters.AddAsync(podCaster);
            await dbContext.SaveChangesAsync();
            return podCaster.PodCasterId;

        }

        public async Task DeletePodCaster(int? PodCasterId, string PodCasterName)
        {

            var PodCaster_Removed = await dbContext.PodCasters.FindAsync(PodCasterId);
         if (PodCaster_Removed is null)
            {
                throw new ResourceNotFound(nameof(PodCaster), PodCasterId.ToString());
            }
            dbContext.Remove(PodCaster_Removed);
            await dbContext.SaveChangesAsync(); 
        }

        public async Task<(int, IEnumerable<PodCaster>)> GetAllPodCasters(string? search, int requestPageNumber, int requestPageSize)
        {

            var query = dbContext.PodCasters.AsQueryable();
            if (!string.IsNullOrEmpty(search))
            { query = query.Where(a => a.Name.Contains(search) || a.About.Contains(search)); }


            //TODO : Pagination validation
            if (requestPageNumber < 1) requestPageNumber = 1;
            if (requestPageSize < 1) requestPageSize = 10; // Default page size

            search ??= string.Empty;
            search = search.ToLower();
            var baseQuery = dbContext.PodCasters
                .Where(r => r.Name.ToLower().Contains(search));
            



            var totalCount = await baseQuery.CountAsync();
            var PodCaster = await baseQuery
                .Skip(requestPageSize * (requestPageNumber - 1))
                .Take(requestPageSize)
                .ToListAsync();
            return (totalCount, PodCaster);





        }

        public Task<PodCaster> GetPodCasterByIdOrName(int? Id, string name)
        {
            throw new NotImplementedException();
        }

        public Task UpdatePodCasterAsync(PodCaster podCaster)
        {
            throw new NotImplementedException();
        }
    }
}
