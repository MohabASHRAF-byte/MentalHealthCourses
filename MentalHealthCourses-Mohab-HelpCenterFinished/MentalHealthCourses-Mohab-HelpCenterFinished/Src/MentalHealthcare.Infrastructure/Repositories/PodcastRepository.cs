using MentalHealthcare.Domain.Entities;
using MentalHealthcare.Domain.Exceptions;
using MentalHealthcare.Domain.Repositories;
using MentalHealthcare.Infrastructure.Persistence;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthcare.Infrastructure.Repositories
{
    public class PodcastRepository(
         MentalHealthDbContext dbContext,
    ILogger<PodcastRepository> logger


        ) : IPodcastRepository
    {
        public async Task<int> AddPodcastAsync(Podcast podcast)
        {
            if (podcast.Title == null)
            {
                throw new ArgumentNullException(nameof(podcast));
            }


            try
            {
                await dbContext.Podcasts.AddAsync(podcast);
                await dbContext.SaveChangesAsync();
            }

            catch (DbUpdateException ex)
            {
                if (ex.InnerException is SqlException sqlException)
                {
                    // Handle specific SQL errors if needed
                    logger.LogError(ex, "An error occurred while saving the pending video upload.");
                }
                logger.LogError(ex, "An error occurred while saving the pending video upload.");
                throw;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while saving the pending video upload.");
                throw;
            }



            return podcast.PodcastId;
        }

        public async Task DeletePodcastAsync(int podcastId)
        {var Selected_podcast = await dbContext.Podcasts
               .FirstOrDefaultAsync(c => c.PodcastId == podcastId);

            if (Selected_podcast == null)
            {
                throw new ResourceNotFound("Video", podcastId.ToString());
            }
            dbContext.Podcasts.Remove(Selected_podcast);
            dbContext.SaveChanges();

            logger.LogInformation($"Podcast with ID {podcastId} has been deleted successfully.");












        }

        public async Task<(int, IEnumerable<Podcast>)> GetAllPodcastsAsync(string? search, int requestPageNumber, int requestPageSize, string? sortBy)
        {
            var query = dbContext.Podcasts.AsQueryable();
            if (!string.IsNullOrEmpty(search))
            { query = query.Where(a => a.Title.Contains(search) || a.Title.Contains(search)); }


            switch (sortBy)
            {
                case "Title":
                    query = query.OrderBy(a => a.Title); break;
                case "CreatedDate":
                    query = query.OrderBy(a => a.CreatedDate); break;
                // Add more cases as needed
                default: query = query.OrderBy(a => a.PodcastId); break;
            }

            //TODO : Pagination validation
            if (requestPageNumber < 1) requestPageNumber = 1;
            if (requestPageSize < 1) requestPageSize = 10; // Default page size

            search ??= string.Empty;
            search = search.ToLower();
            var baseQuery = dbContext.Podcasts
                .Where(r => r.Title.ToLower().Contains(search));
            //TODO : Apply sorting
            baseQuery = sortBy switch
            {
                "title" => baseQuery.OrderBy(r => r.Title),// Default sorting
                "date" => baseQuery.OrderBy(r => r.CreatedDate),
                _ => baseQuery.OrderBy(r => r.Title)
            };



            var totalCount = await baseQuery.CountAsync();
            var podcasts = await baseQuery
                .Skip(requestPageSize * (requestPageNumber - 1))
                .Take(requestPageSize)
                .ToListAsync();
            return (totalCount, podcasts);
        }

        public async Task<Podcast> GetByIdOrTitle(int? PodcastId, string Title_Podcast)
        {
            var Podcast = await dbContext.Podcasts
                .Where(p => p.PodcastId == PodcastId || p.Title.ToLower().Contains(Title_Podcast.ToLower()))
                .FirstOrDefaultAsync();

            if (Podcast == null)
            {
                throw new ResourceNotFound(nameof(Podcast), PodcastId?.ToString() ?? Title_Podcast);
            }

            return Podcast;
        }









    }
}
