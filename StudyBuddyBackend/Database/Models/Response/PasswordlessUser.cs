using System.Collections.Generic;
using System.Linq;
using StudyBuddyBackend.Database.Entities;

namespace StudyBuddyBackend.Database.Models.Response
{
    public class PasswordlessUser
    {
        public PasswordlessUser(User user)
        {
            Username = user.Username;
            FirstName = user.FirstName;
            LastName = user.LastName;
            Email = user.Email;
            Chats = (user.Chats ?? new List<UserChat>()).Select(c => c.Chat);
        }

        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public IEnumerable<Chat> Chats { get; set; }
    }
}
