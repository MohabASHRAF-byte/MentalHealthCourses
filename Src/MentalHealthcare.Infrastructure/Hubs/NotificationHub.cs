//using MediatR;
//using MentalHealthcare.Domain.Constants;
//using MentalHealthcare.Domain.Entities;
//using MentalHealthcare.Infrastructure.Persistence;
//using Microsoft.AspNetCore.SignalR;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Logging;
//using System;
//using System.Collections.Concurrent;
//using System.Threading.Tasks;
//using MentalHealthcare.Domain.Repositories;
//using System.Security.Claims;
//namespace MentalHealthcare.API.Hubs
//{
//    public class NotificationHub : Hub
//    {

//        private readonly IUserConnectionTracker _connectionTracker;

//        public NotificationHub(IUserConnectionTracker connectionTracker)
//        {
//            _connectionTracker = connectionTracker;
//        }




//        public override async Task OnConnectedAsync()
//        {
//            var userId = Context.User?.FindFirstValue(ClaimTypes.NameIdentifier);
//            if (userId != null)
//            {
//                _connectionTracker.AddConnection(userId, Context.ConnectionId);
//            }
//            await base.OnConnectedAsync();
//        }

//        public override async Task OnDisconnectedAsync(Exception? exception)
//        {
//            var userId = Context.User?.FindFirstValue(ClaimTypes.NameIdentifier);
//            if (userId != null)
//            {
//                _connectionTracker.RemoveConnection(userId, Context.ConnectionId);
//            }
//            await base.OnDisconnectedAsync(exception);











//        }
//    }

//}