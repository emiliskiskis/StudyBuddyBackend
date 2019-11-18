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
        }

        public string Username { get; }
        public string FirstName { get; }
        public string LastName { get; }
        public string Email { get; }
    }
}
