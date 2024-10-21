using MentalHealthcare.Domain.Entities;
using MentalHealthcare.Domain.Repositories;
using MentalHealthcare.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MentalHealthcare.Infrastructure.Repositories
{
    public class MeditationRepository : IMeditation
    {
        private readonly Repositories.GenericContentServices<Meditation> _ContentServices;
        private readonly MentalHealthDbContext _dbContext;

        public MeditationRepository(GenericContentServices<Meditation> services, MentalHealthDbContext dbContext)
        {
            _ContentServices = services;
            _dbContext = dbContext;
        }
        public async Task<string> AddMeditationsync(Meditation MeditationMapper)
        {
            var CheckMeditation = _ContentServices.GetTableNoTracking()
                     .Where(X => X.Content.Equals(MeditationMapper.Content))
                     .FirstOrDefaultAsync();
            if (CheckMeditation != null)
            { return "The Content of This Meditation Already Exist"; }

            await _ContentServices.AddAsync(MeditationMapper);
            return "The Article has been Created Successfully!";
        }
        public async Task<string> DeleteMeditationAsync(Meditation MeditationMapper)
        { var Process = _ContentServices.BeginTransaction();
            try {await _ContentServices.DeleteAsync(MeditationMapper);    
            await Process.CommitAsync();
                return "Success Process!";}
            catch{await Process.RollbackAsync();
                return "failed Process!";}}


        public async Task<(int, IEnumerable<Meditation>)> GetAllAsync(string? searchName, int pageNumber, int pageSize)
        {
            searchName ??= string.Empty;
            searchName = searchName.ToLower();
            var baseQuery = _dbContext.Meditations
                .Where(r => r.Title.ToLower().Contains(searchName));
            var totalCount = await baseQuery.CountAsync();
            var Meditation = await baseQuery
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize)
                .ToListAsync();
            return (totalCount, Meditation);
        }
        public async Task<Meditation> GetById(int MeditationId)
        {
            var CheckMeditation = _ContentServices.GetTableNoTracking()
                .Include(x => x.Content)
                .Where(X => X.MeditationId.Equals(MeditationId))
                .FirstOrDefaultAsync();
            return await CheckMeditation ?? null;

        }
        public async Task<bool> IsExist(int MeditationId)
        {

            var MeditationCheck = _ContentServices
                .GetTableNoTracking()
                .Where(a => a.MeditationId
                .Equals(MeditationId)).FirstOrDefault();
            if (MeditationCheck is null)
            {
                return false;//the Article Not Exist Before 
            }
            return true;



        }
        public async Task<bool> IsExistDuringUpdate(string Content, int Id)
        {
            //To Do : Can't Assigne same Content to 2 Different Meditation 
            var MeditationCheck = await _ContentServices
            .GetTableNoTracking()
            .Where(a => a.Content.Equals(Content) & !a.MeditationId.Equals(Id))
            .FirstOrDefaultAsync();
            if (MeditationCheck is null)
            {
                return false;
                await _ContentServices.UpdateAsync(MeditationCheck);
            }

            //Another Meditation Exists with same data
            return true;




        }
        public async Task<string> UpdateMeditationAsync(Meditation MeditationMapper)
        {



            await _ContentServices.UpdateAsync(MeditationMapper);
            return "The Article has been Updated Successfully!";


        }
        public async Task<bool> IsExistByTitle(string title)
        {
            return await _ContentServices
                .GetTableNoTracking()
                .Where(a => a.Title.Equals(title))
                .FirstOrDefaultAsync() != null;
        }
        public async Task<bool> IsExistByContent(string content)
        {
            return await _ContentServices
                .GetTableNoTracking()
                .Where(a => a.Content.Equals(content))
                .FirstOrDefaultAsync() != null;




        }
    }
}
