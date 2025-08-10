//using MentalHealthcare.Domain.Dtos;
//using MentalHealthcare.Domain.Entities;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace MentalHealthcare.Domain.Repositories
//{
//    public interface INotificationRepository
//    {
//        Task<List<Notification>> GetUnreadNotificationsAsync(int userId);
//        Task<int> MarkNotificationsAsReadAsync(int userId, List<int> notificationIds);
//        Task<int> MarkAllNotificationsAsReadAsync(int userId);
//        Task SendNotificationAsync(int userId, string title, string message);
//    }
//}
