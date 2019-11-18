using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StudyBuddyBackend.Database.Entities
{
    public class Chat
    {
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
        // Initializing lists because empty sets in database create null values
        public ICollection<UserInChat> Users { get; set; } = new List<UserInChat>();
        public ICollection<Message> Messages { get; set; } = new List<Message>();

        public Chat()
        {

        }

        public Chat(string id)
        {
            Id = id;
        }
    }
}
