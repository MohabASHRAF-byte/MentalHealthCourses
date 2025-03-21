using MentalHealthcare.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthcare.Domain.Repositories
{
    public interface IMeditationRepository
    {
        public Task<int> CreateMeditationAsync(Meditation Meditation);
        public Task<Meditation> GetMeditationByIdAsync(int Id);
        public Task<(int, IEnumerable<Meditation>)> GetAllMeditationsAsync(string? search, int requestPageNumber, int requestPageSize);
        public Task DeleteMeditationAsync(int MeditationID);
        public Task UpdateMeditationAsync(Meditation meditation);

        public Task SaveChangesAsync();

        public Task<(int TotalCount, IEnumerable<Meditation>)> GetRecentlyAddedMeditationsAsync(int count);
    }
}
