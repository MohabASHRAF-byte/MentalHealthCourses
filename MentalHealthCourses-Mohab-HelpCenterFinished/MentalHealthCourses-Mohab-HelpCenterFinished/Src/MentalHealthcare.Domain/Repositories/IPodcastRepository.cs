using MentalHealthcare.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthcare.Domain.Repositories
{
    public interface IPodcastRepository
    { public Task<int> AddPodcastAsync(Podcast podcast);//Create
        public Task DeletePodcastAsync(int podcastId);//Delete
        public Task<Podcast> GetByIdOrTitle(int? PodcastId, string Title_Podcast);//GetByID
        public Task<(int, IEnumerable<Podcast>)> GetAllPodcastsAsync(string? searchName, int pageNumber, int pageSize , string? sortBy);//GetAll





    }
}
