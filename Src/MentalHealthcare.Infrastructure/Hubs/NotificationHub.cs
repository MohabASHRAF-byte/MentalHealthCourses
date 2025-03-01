using MentalHealthcare.Infrastructure.Persistence;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthcare.Infrastructure.Hubs
{
    public class NotificationHub : Hub
    {
        private readonly ILogger<NotificationHub> _logger;
        private static readonly ConcurrentDictionary<string, string> _userConnections = new ConcurrentDictionary<string, string>();

        public NotificationHub(ILogger<NotificationHub> logger , MentalHealthDbContext dbContext)
        {

            _logger = logger;
        }


        // When a user connects, store their connectionId mapped to userId
        public override Task OnConnectedAsync()
        {
            var userId = Context.GetHttpContext()?.Request?.Query["userId"].ToString();
            if (!string.IsNullOrEmpty(userId))
            {
                _userConnections[userId] = Context.ConnectionId;  // Map userId to connectionId
                _logger.LogInformation($"User {userId} connected with Connection ID: {Context.ConnectionId}");
            }

            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            var userId = _userConnections.FirstOrDefault(x => x.Value == Context.ConnectionId).Key;

            if (userId != null)
            {
                _userConnections.TryRemove(userId, out _); // Remove userId to connectionId mapping
                _logger.LogInformation($"User {userId} disconnected.");
            }

            return base.OnDisconnectedAsync(exception);
        }



        // Admin sends a message to a specific user by userId
        public async Task SendMessageToUser(int adminId, string userId, string message)
        {
            if (_userConnections.TryGetValue(userId, out var connectionId))
            {
                await Clients.Client(connectionId).SendAsync("ReceiveMessage", message);
            }
            else
            {
                _logger.LogWarning($"User {userId} is not connected.");
            }
        }
        // Example: send message to all users (for broadcasting)
        public async Task SendMessageToAll(string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", message);
        }

    }
}
