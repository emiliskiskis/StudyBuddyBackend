using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StudyBuddyBackend.Database.Entities
{
    public class Chat
    {
        [Key]
        public string GroupName { get; set; }
        public ICollection<UserChat> Users { get; set; }
        public ICollection<Message> Messages { get; set; }

        public Chat()
        {

        }

        public Chat(string groupName)
        {
            GroupName = groupName;
        }
    }
}
