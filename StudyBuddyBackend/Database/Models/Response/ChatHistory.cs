using System;
using StudyBuddyBackend.Database.Entities;

namespace StudyBuddyBackend.Database.Models.Response
{
    public class ChatHistory
    {
        public ChatHistory(Message message)
        {
            Id = message.Id;
            Text = message.Text;
            User = new PublicUser(message.User);
            SentAt = message.SentAt;
        }
        
        public string Id { get; }
        public string Text { get; }
        public DateTime SentAt { get; }
        public PublicUser User { get; }
    }
}
