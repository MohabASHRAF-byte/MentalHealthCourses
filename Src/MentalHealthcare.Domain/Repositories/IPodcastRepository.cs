using MentalHealthcare.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthcare.Domain.Repositories
{
    public interface IPodcastRepository
    {

        public Task<int> AddNewEpisodeAsync(Podcast podcast);

        public Task<(int TotalCount, IEnumerable<Podcast>)> GetAllEpisodesAsync(string? search, int requestPageNumber, int requestPageSize , int? podcastSeriesId);

        public Task<Podcast> GetEpisodeByIdAsync(int podcastId);

        public Task SaveChangesAsync();
        public Task DeleteEpisodeAsync(int episodeId);
       public Task UpdateEpisodeAsync(Podcast episode); 

    }
}
