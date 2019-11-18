namespace StudyBuddyBackend.Database.Models.Response
{
    public class Chat
    {
        public string Id { get; }

        public Chat(Entities.Chat chat)
        {
            Id = chat.Id;
        }
    }
}
