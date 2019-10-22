using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StudyBuddyBackend.Database.Entities
{
    public class Chat
    {
        [Key]
        public string GroupName { get; set; }
        public ICollection<UserChat> Users { get; set; } = new List<UserChat>();
        public ICollection<Message> Messages { get; set; } = new List<Message>();

        public Chat()
        {

        }

        public Chat(string groupName)
        {
            GroupName = groupName;
        }
    }
}
