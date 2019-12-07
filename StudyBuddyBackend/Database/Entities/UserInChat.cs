using System.ComponentModel.DataAnnotations.Schema;

namespace StudyBuddyBackend.Database.Entities
{
    // Intermediary table to create a many-to-many relationship of users to chats
    public class UserInChat
    {
        [ForeignKey("Chat")]
        public string ChatId { get; set; }
        [ForeignKey("User")]
        public string Username { get; set; }
        public Chat Chat { get; set; }
        public User User { get; set; }

        public UserInChat()
        {
            
        }

        public UserInChat(User user, Chat chat)
        {
            User = user;
            Chat = chat;
        }
    }
}
