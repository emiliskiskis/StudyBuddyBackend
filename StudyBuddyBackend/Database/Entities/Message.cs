using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudyBuddyBackend.Database.Entities
{
    public enum MessageStatus
    {
        Unanswered,
        WaitingClarification,
        Answered
    }

    public class Message
    {
        [ForeignKey("User")]
        public string Username { get; set; }
        
        [ForeignKey("Chat")]
        public string ChatId { get; set; }
        
        public Chat Chat { get; set; }
        public User User { get; set; }
        public string Text { get; set; }

        // Auto-generated
        public DateTime SentAt { get; set; }
        
        public MessageStatus Status { get; set; }

        public Message()
        {
            
        }

        public Message(string username, string text)
        {
            Username = username;
            Text = text;
            Status = MessageStatus.Unanswered;
        }
    }
}
