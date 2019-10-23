using StudyBuddyBackend.Database.Entities;

namespace StudyBuddyBackend.Database.Models.Response
{
    public class ChatlessUser
    {
        public ChatlessUser(User user)
        {
            Username = user.Username;
            Password = user.Password;
            Salt = user.Salt;
            FirstName = user.FirstName;
            LastName = user.LastName;
            Email = user.Email;
        }

        public string Username { get; }
        public string Password { get; }
        public string Salt { get; }
        public string FirstName { get; }
        public string LastName { get; }
        public string Email { get; }
    }
}
