using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentalHealthcare.Domain.Repositories
{
    public interface IUserConnectionTracker
    {
        void AddConnection(string userId, string connectionId);
        void RemoveConnection(string userId, string connectionId);
        IEnumerable<string> GetConnections(string userId);
    }





    public class UserConnectionTracker : IUserConnectionTracker
    {
        private readonly ConcurrentDictionary<string, List<string>> _userConnections = new();

        public void AddConnection(string userId, string connectionId)
        {
            _userConnections.AddOrUpdate(userId,
                new List<string> { connectionId },
                (_, existing) => { existing.Add(connectionId); return existing; });
        }

        public void RemoveConnection(string userId, string connectionId)
        {
            if (_userConnections.TryGetValue(userId, out var connections))
            {
                connections.Remove(connectionId);
                if (connections.Count == 0)
                {
                    _userConnections.TryRemove(userId, out _);
                }
            }
        }

        public IEnumerable<string> GetConnections(string userId)
        {
            return _userConnections.TryGetValue(userId, out var connections)
                ? connections
                : Enumerable.Empty<string>();
        }
    }




}
