using System;
using System.Threading.Tasks;

namespace StudyBuddyBackend.Hubs
{
    public interface IHubClient
    {
        Task ReceiveMessage(string username, string chatId, string messageId, string messageText, DateTime sentAt);
        Task ReceiveChat(string chatId);
    }
}
