using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StudyBuddyBackend.Database.Entities
{
    public class Chat
    {
        [Key]
        public string GroupName { get; set; }
        // Initializing lists because empty sets in database create null values
        public virtual ICollection<UserInChat> Users { get; set; } = new List<UserInChat>();
        public virtual ICollection<Message> Messages { get; set; } = new List<Message>();

        public Chat()
        {

        }

        public Chat(string groupName)
        {
            GroupName = groupName;
        }
    }
}
