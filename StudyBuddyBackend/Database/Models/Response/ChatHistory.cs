using System;
using StudyBuddyBackend.Database.Entities;

namespace StudyBuddyBackend.Database.Models.Response
{
    public class ChatHistory
    {
        public ChatHistory(Message message) {
            Text = message.Text;
            User = new PublicUser(message.User);
            SentAt = message.SentAt;
        }

        public string Text { get; }
        public DateTime SentAt { get; }
        public PublicUser User { get; }
    }
}
