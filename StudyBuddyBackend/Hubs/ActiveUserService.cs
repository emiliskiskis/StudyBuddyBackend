using System.Collections.Generic;

namespace StudyBuddyBackend.Hubs
{
    public class ActiveUser
    {
        internal string ConnectionId { get; }
        internal string Username { get; }

        internal ActiveUser(string connectionId, string username)
        {
            ConnectionId = connectionId;
            Username = username;
        }
    }
    
    public class ActiveUserService
    {
        public ICollection<ActiveUser> ActiveUsers { get; } = new List<ActiveUser>();
    }
}
