using MentalHealthcare.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthcare.Domain.Repositories
{
    public interface IPodCasterRepository
    {

        public Task<int> AddPodCaster(PodCaster podCaster);
        public Task DeletePodCaster(int? PodCasterId, string PodCasterName);
        public Task UpdatePodCasterAsync(PodCaster podCaster);

        public Task<(int, IEnumerable<PodCaster>)> GetAllPodCasters(string? search, int requestPageNumber, int requestPageSize);
        public Task<PodCaster> GetPodCasterByIdOrName(int? Id, string name);











    }
}
