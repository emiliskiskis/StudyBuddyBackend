using System.Collections.Generic;

namespace StudyBuddyBackend.Database.Models.Response
{
    public class Chat
    {
        public string Id { get; }
        public ChatHistory LastMessage { get; }
        public IEnumerable<PublicUser> Users { get; }

        public Chat(Entities.Chat chat, ChatHistory lastMessage, IEnumerable<PublicUser> users)
        {
            Id = chat.Id;
            LastMessage = lastMessage;
            Users = users;
        }
    }
}
