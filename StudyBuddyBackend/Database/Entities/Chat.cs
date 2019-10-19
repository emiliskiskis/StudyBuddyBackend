using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudyBuddyBackend.Database.Entities
{
    public class Chat
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public ICollection<UserChat> Users { get; set; }
        public ICollection<Message> Messages { get; set; }
    }
}
