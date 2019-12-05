using System.Collections.Generic;
using System.Linq;

namespace StudyBuddyBackend.Database.Models.Response
{
    public class Chat
    {
        public string Id { get; }
        public ChatHistory LastMessage { get; }
        public IEnumerable<PublicUser> Users { get; }

        public Chat(Entities.Chat chat)
        {
            Id = chat.Id;
            var lastMessage = chat.Messages.OrderBy(m => m.SentAt).LastOrDefault();
            LastMessage = lastMessage != default ? new ChatHistory(lastMessage) : default;
            Users = chat.Users.Select(u => new PublicUser(u.User));
        }
    }
}
