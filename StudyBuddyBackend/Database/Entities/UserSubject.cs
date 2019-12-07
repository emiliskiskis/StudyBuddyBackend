using System.ComponentModel.DataAnnotations.Schema;

namespace StudyBuddyBackend.Database.Entities
{
    public class UserSubject
    {
        [ForeignKey("Subject")]
        public string SubjectName { get; set; }
        [ForeignKey("User")]
        public string Username { get; set; }
        public Subject Subject { get; set; }
        public User User { get; set; }

        public UserSubject()
        {

        }

        public UserSubject(User user, Subject subject)
        {
            User = user;
            Subject = subject;
        }
    }
}
