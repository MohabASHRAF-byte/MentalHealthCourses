using MentalHealthcare.Domain.Entities;

namespace MentalHealthcare.Domain.Repositories
{
    public interface IMeditation 
    {
        public Task<string> AddMeditationsync(Meditation MeditationMapper);//Create
        public Task<string> DeleteMeditationAsync(Meditation MeditationMapper);//Delete
        public Task<Meditation> GetById(int MeditationId);//GetByID
        public Task<(int, IEnumerable<Meditation>)> GetAllAsync(string? searchName, int pageNumber, int pageSize);//GetAll
       public Task<string> UpdateMeditationAsync(Meditation MeditationMapper);//Update
        public Task<bool> IsExist(int MeditationId);//Check Existance of Entity
        public Task<bool> IsExistDuringUpdate(string Content, int Id);//Check Existance of Entity Before Insert New Data

        public Task<bool> IsExistByTitle(string title);

        public Task<bool> IsExistByContent(string content);

    }
}
