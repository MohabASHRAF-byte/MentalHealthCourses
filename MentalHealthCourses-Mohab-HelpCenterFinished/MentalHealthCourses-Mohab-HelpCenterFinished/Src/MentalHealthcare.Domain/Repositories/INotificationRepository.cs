using MentalHealthcare.Domain.Entities;

namespace MentalHealthcare.Domain.Repositories
{
    public interface INotificationRepository
    {


        Task AddAsync(Notification notification); 
        Task<IEnumerable<Notification>> GetAllAsync();
        Task<Notification> GetByIdAsync(int id); 




    }
}
