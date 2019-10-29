using System.ComponentModel.DataAnnotations.Schema;

namespace StudyBuddyBackend.Database.Entities
{
    // Intermediary table to create a many-to-many relationship of users to chats
    public class UserInChat
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public virtual Chat Chat { get; set; }
        public virtual User User { get; set; }

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
