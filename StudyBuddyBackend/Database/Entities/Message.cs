using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudyBuddyBackend.Database.Entities
{
    public class Message
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public virtual User User { get; set; }
        public string Text { get; set; }

        // Auto-generated
        public DateTime SentAt { get; set; }

        public Message()
        {
            
        }

        public Message(User user, string text)
        {
            User = user;
            Text = text;
        }
    }
}
