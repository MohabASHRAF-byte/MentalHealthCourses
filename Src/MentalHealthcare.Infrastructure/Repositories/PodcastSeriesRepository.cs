using MentalHealthcare.Domain.Entities;
using MentalHealthcare.Domain.Entities.Courses;
using MentalHealthcare.Domain.Exceptions;
using MentalHealthcare.Domain.Repositories;
using MentalHealthcare.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthcare.Infrastructure.Repositories
{
    public class PodcastSeriesRepository(MentalHealthDbContext dbContext ,
        ILogger<PodcastSeriesRepository>logger) : IPodcastSeriesRepository
    {
        public async Task<int> CreatePodcastSeriesAsync(PodcastSeries Series)
        {

            await dbContext.podcastSeries.AddAsync(Series);

            await dbContext.SaveChangesAsync();

            return Series.PodcastSeriesId;



        }

        public async Task DeletePodcastSeriesAsync(int podcastSeriesId)
        {
            var Series = await dbContext.podcastSeries.FindAsync(podcastSeriesId);
            if (Series == null)
            {
                throw new ResourceNotFound("PodcastSeries", "السلسله غير موجود", podcastSeriesId.ToString());
            }

            dbContext.podcastSeries.Remove(Series);
            await dbContext.SaveChangesAsync();
        }

        public async Task<(int TotalCount, IEnumerable<PodcastSeries>)> GetAllPodcastSeriesAsync(string? search, int requestPageNumber, int requestPageSize)
        {


            try
            {
                var baseQuery = dbContext.podcastSeries
                    .Include(ps => ps.Podcasts) // Include Podcasts for eager loading
                    .AsQueryable();

                // Apply search filter
                if (!string.IsNullOrWhiteSpace(search))
                {
                    baseQuery = baseQuery.Where(ps => ps.NameOfTopic.Contains(search) ||
                                                      ps.PodcasterName.Contains(search));
                }

                // Get total count before pagination
                var totalCount = await baseQuery.CountAsync();

                // Apply pagination & projection
                var series = await baseQuery
                    .OrderBy(ps => ps.PodcastSeriesId)
                    .Skip(requestPageSize * (requestPageNumber - 1))
                    .Take(requestPageSize)
                    .Select(ps => new PodcastSeries
                    {
                        PodcastSeriesId = ps.PodcastSeriesId,
                        NameOfTopic = ps.NameOfTopic,
                        Image = ps.Image,
                        PodcasterName = ps.PodcasterName,
                        PodcastDescription = ps.PodcastDescription,
                        Podcasts = ps.Podcasts.Select(p => new Podcast
                        {
                            PodcastId = p.PodcastId,
                            PodcastSeriesId = p.PodcastSeriesId,
                            Title = p.Title,
                            EpisodeLength = p.EpisodeLength,
                            AudioFile = p.AudioFile
                        }).ToList()
                    })
                    .ToListAsync();

                return (totalCount, series);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error retrieving podcast series.");
                throw;
            }














        }


        public async Task<PodcastSeries?> GetPodcastSeriesByIdAsync(int seriesId)
        {
            var Series = await dbContext.podcastSeries
  .Where(a => a.PodcastSeriesId == seriesId)
  .Select(a => new PodcastSeries
  {
      PodcastSeriesId = a.PodcastSeriesId,
      NameOfTopic = a.NameOfTopic,
      PodcasterName = a.PodcasterName,
      PodcastDescription = a.PodcastDescription,
      Image = a.Image,
      Podcasts = a.Podcasts.OrderBy(p => p.PodcastId)
          .Select(p => new Podcast {
              PodcastId = p.PodcastId,
              Title = p.Title,
              PodcastSeriesId = p.PodcastSeriesId,
              EpisodeLength = p.EpisodeLength,
              AudioFile = p.AudioFile
          })
          .ToList(),

  })
  .FirstOrDefaultAsync();


            if (Series == null)
                throw new ResourceNotFound(
                    "PodcastSeries",
                    "السلسله غير موجود",
                    seriesId.ToString()
                ); ; return Series;  }

        public Task<(int TotalCount, IEnumerable<PodcastSeries>)> GetRecentlyAddedPodcastSeriesAsync(int count)
        {
            throw new NotImplementedException();
        }

        public async Task SaveChangesAsync()
        {
            await dbContext.SaveChangesAsync();
        }

        public async Task UpdatePodcastSeriesAsync(PodcastSeries Series)
        {
            dbContext.podcastSeries.Update(Series);
            await dbContext.SaveChangesAsync();
        }
    }
}
