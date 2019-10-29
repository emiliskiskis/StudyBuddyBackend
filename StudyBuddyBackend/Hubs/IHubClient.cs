using System;
using System.Threading.Tasks;

namespace StudyBuddyBackend.Hubs
{
    public interface IHubClient
    {
        Task ReceiveMessage(string username, string groupName, string messageText, DateTime sentAt);
    }
}
