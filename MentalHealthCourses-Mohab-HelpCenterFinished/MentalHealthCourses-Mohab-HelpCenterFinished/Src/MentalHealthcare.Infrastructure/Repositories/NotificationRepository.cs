using MentalHealthcare.Domain.Entities;
using MentalHealthcare.Domain.Repositories;
using MentalHealthcare.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MentalHealthcare.Infrastructure.Repositories
{
    public class NotificationRepository(MentalHealthDbContext dbContext) : INotificationRepository
    {
       
        public async Task AddAsync(Notification notification)
        {
            await dbContext.Notifications.AddAsync(notification);
            await dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Notification>> GetAllAsync()
        {
            return await dbContext.Notifications.ToListAsync();
        }

        public async Task<Notification> GetByIdAsync(int id)
        {
            return await dbContext.Notifications.FindAsync(id);
        }

        // Other CRUD methods as needed
    }
}
