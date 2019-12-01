using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudyBuddyBackend.Database.Entities
{
    public class Message
    {
        [Column(TypeName = "VARCHAR(36)")]
        public string Id { get; set; }
        public User User { get; set; }
        public string Text { get; set; }

        // Auto-generated
        public DateTime SentAt { get; set; }

        public Message()
        {
            
        }

        public Message(string id, User user, string text)
        {
            Id = id;
            User = user;
            Text = text;
        }
    }
}
