using System.Collections.Generic;

namespace StudyBuddyBackend.Database.Models.Response
{
    public class Chat
    {
        public string Id { get; }
        public IEnumerable<PublicUser> Users { get; }

        public Chat(Entities.Chat chat, IEnumerable<PublicUser> users)
        {
            Id = chat.Id;
            Users = users;
        }
    }
}
