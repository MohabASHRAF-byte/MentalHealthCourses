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
    public class PodCasterRepository(MentalHealthDbContext dbContext) : IPodCastRepository
    {
        public async Task<int> CreateAsync(Podcast podcast)
        {
            await dbContext.Podcasts.AddAsync(podcast);
            await dbContext.SaveChangesAsync();
            return podcast.PodCasterId;
        }

        public async Task DeletePodcastAsync(int ID)
        {


            var newPodcast = await dbContext.Podcasts.FindAsync(ID);
            if (newPodcast == null)
                throw new ResourceNotFound(nameof(newPodcast), ID.ToString());
            dbContext.Podcasts.Remove(newPodcast);
            await dbContext.SaveChangesAsync();


        }

        public async Task<(int, IEnumerable<Podcast>)> GetAllPodcastsAsync(string? search, int requestPageNumber, int requestPageSize, string? sortBy)
        {
            // Default pagination values
            requestPageNumber = Math.Max(requestPageNumber, 1);
            requestPageSize = Math.Max(requestPageSize, 10);

            // Normalize search term
            search = search?.ToLowerInvariant() ?? string.Empty;

            // Build query
            var query = dbContext.Podcasts.AsQueryable();

            // Apply search filter
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(a => a.Title.ToLower().Contains(search));
            }

            // Apply sorting
            query = sortBy?.ToLower() switch
            {
                "title" => query.OrderBy(p => p.Title),
                "createddate" => query.OrderBy(p => p.CreatedDate),
                _ => query.OrderBy(p => p.PodcastId)
            };

            // Get total count before applying pagination
            var totalCount = await query.CountAsync();

            // Apply pagination
            var podcasts = await query
                .Skip(requestPageSize * (requestPageNumber - 1))
                .Take(requestPageSize)
                .ToListAsync();

            return (totalCount, podcasts);
        }

        public async Task<Podcast> GetPodcastByIdAsync(int Id)
        {
            var Podcast = await dbContext.Podcasts
                       .Where(a => a.PodcastId == Id)
                       .Select(a => new Podcast
                       {
                           PodcastId = a.PodcastId,
                           Title = a.Title,
                           PodcastDescription = a.PodcastDescription,
                           Url = a.Url
                        
                       })
                       .FirstOrDefaultAsync();

            if (Podcast == null)
                throw new ResourceNotFound(nameof(Advertisement),Id.ToString());

            return Podcast;








        }

      
    }
}
