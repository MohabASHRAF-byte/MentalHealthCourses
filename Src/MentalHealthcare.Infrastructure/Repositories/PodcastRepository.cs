using MentalHealthcare.Domain.Entities;
using MentalHealthcare.Domain.Exceptions;
using MentalHealthcare.Domain.Repositories;
using MentalHealthcare.Infrastructure.Persistence;
using MentalHealthcare.Infrastructure.Repositories.Course;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace MentalHealthcare.Infrastructure.Repositories
{
    public class PodcastRepository (MentalHealthDbContext dbContext,
         ILogger<PodcastRepository> logger
         ) : IPodcastRepository
    {
        public async Task<int> AddNewEpisodeAsync(Podcast podcast)
        { try
            { var series = await dbContext.podcastSeries
            .FirstOrDefaultAsync(ps => ps.PodcastSeriesId == podcast.PodcastSeriesId);
        if (series == null)
                {logger.LogWarning("PodcastSeries with ID {PodcastSeriesId} not found.", podcast.PodcastSeriesId);
                    throw new KeyNotFoundException("PodcastSeries not found."); }

                await dbContext.Podcasts.AddAsync(podcast);
                await dbContext.SaveChangesAsync();

                return podcast.PodcastId;
            }
            catch (Exception ex)
            { logger.LogError(ex, "An error occurred while saving the podcast.");
                throw;
            }



        }

        public async Task DeleteEpisodeAsync(int episodeId)
        {
            var pod = await dbContext.Podcasts.FindAsync(episodeId);
            if (pod == null)
                throw new ResourceNotFound(
                    "PodCaster",
                    "السلسله",
                    episodeId.ToString()
                );

            dbContext.Podcasts.Remove(pod);
            await dbContext.SaveChangesAsync();
        }

        public async Task<(int TotalCount, IEnumerable<Podcast>)> GetAllEpisodesAsync(string? search, int requestPageNumber, int requestPageSize , int? podcastSeriesId)
        {
            try
            {
                var baseQuery = dbContext.Podcasts
                    .Include(p => p.podcastSeries) // Include PodcastSeries for related data
                    .AsQueryable();
                // 🔹 Filter by PodcastSeriesId if provided
                if (podcastSeriesId.HasValue)
                {
                    baseQuery = baseQuery.Where(e => e.PodcastSeriesId == podcastSeriesId.Value);
                }
                // Apply search filter
                if (!string.IsNullOrWhiteSpace(search))
                {
                    baseQuery = baseQuery.Where(p => p.Title.Contains(search) ||
                                                     p.podcastSeries.NameOfTopic.Contains(search) ||
                                                     p.podcastSeries.PodcasterName.Contains(search));
                }

                // Get total count before pagination
                int totalCount = await baseQuery.CountAsync();

                // Apply pagination & projection
                var podcasts = await baseQuery
                    .OrderByDescending(p => p.CreatedDate) // Assuming you want latest first
                    .Skip(requestPageSize * (requestPageNumber - 1))
                    .Take(requestPageSize)
                    .Select(p => new Podcast
                    {
                        PodcastId = p.PodcastId,
                        Title = p.Title,
                        AudioFile = p.AudioFile,
                        EpisodeLength = p.EpisodeLength,
                        PodcastSeriesId = p.PodcastSeriesId,
                        podcastSeries = new PodcastSeries
                        {
                            PodcastSeriesId = p.podcastSeries.PodcastSeriesId,
                            NameOfTopic = p.podcastSeries.NameOfTopic,
                            PodcasterName = p.podcastSeries.PodcasterName
                        }
                    })
                    .ToListAsync();

                return (totalCount, podcasts);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error retrieving podcasts.");
                throw;
            }
        }

        public async Task<Podcast> GetEpisodeByIdAsync(int podcastId)
        {
            try
            {
                var podcast = await dbContext.Podcasts
                    .Include(p => p.podcastSeries) // Include related PodcastSeries
                    .Where(p => p.PodcastId == podcastId)
                    .Select(p => new Podcast
                    {
                        PodcastId = p.PodcastId,
                        Title = p.Title,
                        AudioFile = p.AudioFile,
                        EpisodeLength = p.EpisodeLength,
                        PodcastSeriesId = p.PodcastSeriesId,
                        podcastSeries = new PodcastSeries
                        {
                            PodcastSeriesId = p.podcastSeries.PodcastSeriesId,
                            NameOfTopic = p.podcastSeries.NameOfTopic,
                            PodcasterName = p.podcastSeries.PodcasterName
                        }
                    })
                    .FirstOrDefaultAsync();

                return podcast;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error retrieving podcast with ID {podcastId}");
                throw;
            }




          
        }

        public async Task SaveChangesAsync()
        {
         
                await dbContext.SaveChangesAsync();
          
        }

        public async Task UpdateEpisodeAsync(Podcast episode)
        {
            dbContext.Podcasts.Update(episode);
            await dbContext.SaveChangesAsync();

        }
    }
}
