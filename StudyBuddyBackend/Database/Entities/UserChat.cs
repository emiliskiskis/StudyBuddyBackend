using System.ComponentModel.DataAnnotations.Schema;

namespace StudyBuddyBackend.Database.Entities
{
    // Intermediary table to create a many-to-many relationship of users to chats
    public class UserChat
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public Chat Chat { get; set; }
        public User User { get; set; }

        public UserChat()
        {
            
        }

        public UserChat(User user, Chat chat)
        {
            User = user;
            Chat = chat;
        }
    }
}
