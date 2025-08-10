//using MediatR;
//using MentalHealthcare.API.Hubs;
//using MentalHealthcare.Domain.Dtos;
//using MentalHealthcare.Domain.Entities;
//using MentalHealthcare.Domain.Exceptions;
//using MentalHealthcare.Domain.Repositories;
////using MentalHealthcare.Infrastructure.NotificationService;
//using MentalHealthcare.Infrastructure.Persistence;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.SignalR;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Logging;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
//namespace MentalHealthcare.Infrastructure.Repositories
//{
//    public class NotificationRepository : INotificationRepository
//    {



//        private readonly MentalHealthDbContext _context;
//        private readonly IHubContext<NotificationHub> _hubContext;
//        private readonly IUserConnectionTracker _connectionTracker;
//        private readonly ILogger<NotificationRepository> _logger;


//        public NotificationRepository(MentalHealthDbContext context, IHubContext<NotificationHub> hubContext,
//            IUserConnectionTracker userConnectionTracker, ILogger<NotificationRepository> logger)
//        {
//            _context = context;
//            _hubContext = hubContext;
//            _connectionTracker = userConnectionTracker;
//            _logger = logger;
//        }




//        public List<Notification> GetAll() =>
//                _context.Notifications.ToList();

//        public Notification GetById(int id) =>
//            _context.Notifications.Find(id);



//        public async Task<List<Notification>> GetUnreadNotificationsAsync(int userId)
//        {
//            return await _context.Notifications
//                            .Where(n => n.UserId == userId && !n.IsRead)
//                            .OrderByDescending(n => n.CreatedAt)
//                            .ToListAsync();
//        }

//        public async Task<int> MarkAllNotificationsAsReadAsync(int userId)
//        {
//            var result = await _context.Notifications
//                            .Where(n => n.UserId == userId && !n.IsRead)
//                            .ExecuteUpdateAsync(setters =>
//                                setters.SetProperty(n => n.IsRead, true));

//            return result;
//        }

//        public async Task<int> MarkNotificationsAsReadAsync(int userId, List<int> notificationIds)
//        {
//            var notifications = await _context.Notifications
//                           .Where(n => n.UserId == userId &&
//                                      !n.IsRead &&
//                                      notificationIds.Contains(n.Id))
//                           .ToListAsync();

//            foreach (var notification in notifications)
//            {
//                notification.IsRead = true;
//            }

//            return await _context.SaveChangesAsync();
//        }

//        public async Task SendNotificationAsync(int userId, string title, string message)
//        {
//            var connections = _connectionTracker.GetConnections(userId.ToString());
//            foreach (var connectionId in connections)
//            {
//                await _hubContext.Clients.Client(connectionId).SendAsync("ReceiveNotification",
//                    new { Title = title, Message = message });
//            }
//        }

//        public void insert(Notification obj) =>
//                        _context.Notifications.Add(obj);

//        public void Update(Notification obj) =>
//            _context.Notifications.Update(obj);

//        public void Delete(Notification obj) =>
//            _context.Notifications.Remove(obj);

//        public void Save() => _context.SaveChanges();



//        public async Task NotifyUserViaSignalR(string userId, string message, object data)
//        {
//            await _hubContext.Clients.User(userId).SendAsync("ReceiveNotification", new
//            {
//                Message = message,
//                Data = data
//            });
//        }







//    }
//}

