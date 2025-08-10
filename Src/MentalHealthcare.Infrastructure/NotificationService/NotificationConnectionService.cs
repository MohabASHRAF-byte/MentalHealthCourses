//using System;
//using System.Collections.Concurrent;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace MentalHealthcare.Infrastructure.NotificationService
//{
//    public class NotificationConnectionService
//    {
//        private static readonly ConcurrentDictionary<string, string> _userConnections = new();

//        public void AddConnection(string userId, string connectionId)
//        {
//            _userConnections[userId] = connectionId;
//        }

//        public void RemoveConnection(string userId)
//        {
//            _userConnections.TryRemove(userId, out _);
//        }

//        public bool TryGetConnectionId(string userId, out string? connectionId)
//        {
//            return _userConnections.TryGetValue(userId, out connectionId);
//        }
//    }
//}
