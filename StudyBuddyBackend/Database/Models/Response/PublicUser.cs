using StudyBuddyBackend.Database.Entities;

namespace StudyBuddyBackend.Database.Models.Response
{
    public class PublicUser
    {
        public PublicUser(User user)
        {
            Username = user.Username;
            FirstName = user.FirstName;
            LastName = user.LastName;
        }
        
        public string Username { get; }
        public string FirstName { get; }
        public string LastName { get; }
    }
}
