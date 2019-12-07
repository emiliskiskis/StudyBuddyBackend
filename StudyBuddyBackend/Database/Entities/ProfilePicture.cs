using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudyBuddyBackend.Database.Entities
{
    public class ProfilePicture
    {
        [Key]
        [ForeignKey("User")]
        public string Username { get; set; }
        public User User { get; set; }
        public string Data { get; set; }

        public ProfilePicture()
        {
            
        }

        public ProfilePicture(string username, string data)
        {
            Username = username;
            Data = data;
        }
    }
}
