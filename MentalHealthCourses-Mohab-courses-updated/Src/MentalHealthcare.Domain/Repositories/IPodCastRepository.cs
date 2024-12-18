using MentalHealthcare.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthcare.Domain.Repositories
{
    public interface IPodCastRepository
    {





        public Task<int> CreateAsync(Podcast podcast);
        public Task<Podcast> GetPodcastByIdAsync(int Id);
        public Task<(int, IEnumerable<Podcast>)> GetAllPodcastsAsync(string? search, int requestPageNumber, int requestPageSize, string? sortBy);
        public Task DeletePodcastAsync(int ID);
















    }
}
