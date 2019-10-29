using StudyBuddyBackend.Database.Entities;

namespace StudyBuddyBackend.Database.Models.Response
{
    public class Group
    {
        public string GroupName { get; }

        public Group(Chat chat)
        {
            GroupName = chat.GroupName;
        }
    }
}
