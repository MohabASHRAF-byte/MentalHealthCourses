using MentalHealthcare.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthcare.Domain.Repositories
{
    public interface IPodcastSeriesRepository
    {


        public Task<int> CreatePodcastSeriesAsync(PodcastSeries Series);
        public Task<PodcastSeries?> GetPodcastSeriesByIdAsync(int SeriesId);
        public Task UpdatePodcastSeriesAsync(PodcastSeries Series);
        public Task DeletePodcastSeriesAsync(int podcastSeriesId);
        public Task<(int TotalCount, IEnumerable<PodcastSeries>)> GetAllPodcastSeriesAsync(string? search, int requestPageNumber, int requestPageSize);
        public Task SaveChangesAsync();

        public Task<(int TotalCount, IEnumerable<PodcastSeries>)> GetRecentlyAddedPodcastSeriesAsync(int count);






    }
}
