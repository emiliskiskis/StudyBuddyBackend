using System;

namespace StudyBuddyBackend.Hubs
{
    public interface HubClientInterface
    {
        void ReceiveMessage(string username, string groupName, string messageText, DateTime sentAt);
    }
}
